using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Board board;
    public GameData gameData;
    public MyInput myInput = new MyInput();
    public PreviewGroup previewGroup = new PreviewGroup();
    public FireGroup fireGroup = new FireGroup();
    public MoveGroup moveGroup = new MoveGroup();
    public void Init(GameData gameData)
    {
        this.gameData = gameData;
        this.myInput.Init(this);
        this.board.Init(this);
        this.previewGroup.Init(this);
        this.fireGroup.Init(this);
        this.moveGroup.Init(this);
    }

    public float time
    {
        get
        {
            return Time.time;
        }
    }

    void Update()
    {
        this.MyUpdate(Time.deltaTime);
    }

    public void MyUpdate(float dt)
    {
        this.myInput.MyUpdate(dt);

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                Cell cell = board.At(i, j);
                cell.MyUpdate(dt);
            }
        }

        //
        this.HandleDirty();
    }

    bool dirty = true;
    public void SetDirty()
    {
        this.dirty = true;
    }
    void HandleDirty()
    {
        if (!this.dirty)
        {
            return;
        }
        this.dirty = false;

        this.gameData.RefreshLink();
        this.board.Refresh();

        if (this.fireGroup.firing)
        {
            return;
        }

        if (this.previewGroup.previewing)
        {
            this.previewGroup.UpdatePreview(this.gameData.boardData.previewGroupDatas);
        }

        if (this.previewGroup.previewing)
        {
            return;
        }

        if (this.gameData.boardData.previewGroupDatas.Count == 0)
        {
            return;
        }

        this.previewGroup.Start(this.gameData.boardData.previewGroupDatas[0], this.OnPreviewFinish);
    }

    void OnCellRotateFinish(Cell cell, RotateDir rotateDir)
    {
        this.SetDirty();
        this.HandleDirty();
    }

    public void OnClick(int x, int y, RotateDir rotateDir)
    {
        Debug.Log($"Click ({x}, {y})");

        Cell cell = this.board.At(x, y);
        if (!cell.state.AskRotate())
        {
            return;
        }

        cell.Rotate(rotateDir, this.OnCellRotateFinish);
        this.SetDirty();
        this.HandleDirty();
    }

    public void OnSwipe(Dir? prevDir, Vector2Int prevPos, Dir dir)
    {
        Debug.Log($"OnSwipe {prevDir} {prevPos} {dir}");

        bool dirty = false;

        // 1

        Cell cell1 = this.board.At(prevPos);
        CellData data1 = this.gameData.boardData.At(prevPos);

        bool needRotate1;
        RotateDir rotateDir1;
        bool canLink1 = prevDir == null
            ? data1.shape.CanLinkTo(dir, out needRotate1, out rotateDir1)
            : data1.shape.CanLinkTo(dir, prevDir.Value.Reverse(), out needRotate1, out rotateDir1);
        if (canLink1 && needRotate1 && cell1.state.AskRotate())
        {
            cell1.Rotate(rotateDir1, this.OnCellRotateFinish);
            dirty = true;
        }

        // 2

        Vector2Int pos2 = prevPos + dir.ToOffset();
        if (this.board.boardData.InRange(pos2))
        {
            Cell cell2 = this.board.At(pos2);
            CellData data2 = this.gameData.boardData.At(pos2);

            bool canLinkTo2 = data2.shape.CanLinkTo(dir.Reverse(), out bool needRotate2, out RotateDir rotateDir2);
            if (canLinkTo2 && needRotate2 && cell2.state.AskRotate())
            {
                cell2.Rotate(rotateDir2, this.OnCellRotateFinish);
                dirty = true;
            }
        }

        if (dirty)
        {
            this.SetDirty();
            this.HandleDirty();
        }
    }

    void OnPreviewFinish(List<Vector2Int> poses)
    {
        Debug.Assert(!this.fireGroup.firing);
        if (!this.fireGroup.firing)
        {
            this.fireGroup.Start(poses, this.OnFireFinish);
        }
    }

    void OnFireFinish(List<Vector2Int> poses)
    {
        this.moveGroup.Move(poses, this.OnCellMoveFinish);
        this.SetDirty();
        this.HandleDirty();
    }

    void OnCellMoveFinish(Cell _cell)
    {
        this.SetDirty();
        this.HandleDirty();
    }
}