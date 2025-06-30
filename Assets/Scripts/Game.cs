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

    public void OnSwipe(int x, int y, Dir dir)
    {
        Cell cell = this.board.At(x, y);

        Vector2Int offset = dir.ToOffset();
        int x2 = x + offset.x;
        int y2 = y + offset.y;
        Cell cell2 = this.board.At(x2, y2);

        if (cell.state.AskRotate())
        {

        }

        if (cell2.state.AskRotate())
        {
            
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