using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Board board;
    public GameData gameData;
    public PreviewGroup previewGroup = new PreviewGroup();
    public FireGroup fireGroup = new FireGroup();
    public void Init(GameData gameData)
    {
        this.gameData = gameData;
        this.board.Init(gameData.boardData);
        this.previewGroup.Init(this);
        this.fireGroup.Init(this);
    }

    void Update()
    {
        this.MyUpdate(Time.deltaTime);
    }

    public void MyUpdate(float dt)
    {
        bool clickL = Input.GetMouseButtonDown(0);
        bool clickR = Input.GetMouseButtonDown(1);
        if (clickL || clickR)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log(mousePos);

            float x = mousePos.x;
            float y = mousePos.y;
            if (x > -this.board.width * 0.5f && x < this.board.width * 0.5f)
            {
                if (y > -this.board.height * 0.5f && y < this.board.height * 0.5f)
                {
                    int i = (int)(x - -this.board.width * 0.5f);
                    int j = (int)(y - -this.board.height * 0.5f);

                    this.OnClick(i, j, clickL ? ClickAction.RotateCCW : ClickAction.RotateCW);
                }
            }
        }

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
        if (this.dirty)
        {
            this.dirty = false;

            this.gameData.RefreshLink();
            this.board.Apply();

            this.RefreshPreviewOrFireQueue();
        }
    }

    void OnCellRotateFinish(Cell cell, RotateDir rotateDir)
    {
        cell.ResetRotation();

        CellData cellData = this.gameData.boardData.At(cell.x, cell.y);
        cellData.forbidLink = false;

        Shape pre = cellData.shape;

        cellData.shape = rotateDir == RotateDir.CW
            ? cellData.shape.GetSettings().rotateCW
            : cellData.shape.GetSettings().rotateCCW;

        Debug.Log($"({cell.x}, {cell.y}) Rotate {pre} -> {cellData.shape}");

        this.SetDirty();
    }

    void OnClick(int i, int j, ClickAction action)
    {
        Debug.Log($"Click ({i}, {j})");

        Cell cell = this.board.At(i, j);
        if (cell.rotating)
        {
            cell.FinishRotate();
        }

        CellData cellData = this.gameData.boardData.At(i, j);
        cellData.forbidLink = true;
        cell.Rotate(action == ClickAction.RotateCW ? RotateDir.CW : RotateDir.CCW, this.OnCellRotateFinish);
        this.SetDirty();
    }

    void RefreshPreviewOrFireQueue()
    {
        if (this.fireGroup.firing)
        {
            return;
        }

        if (this.previewGroup.previewing)
        {
            // try to cancel preview state
            if (this.gameData.boardData.previewGroupDatas.Count == 0)
            {
                this.previewGroup.Cancel();
            }
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
        // Debug.Log("Start preview");
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
        // Debug.Log("OnFireFinish");

        // shift
        // 

    }
}