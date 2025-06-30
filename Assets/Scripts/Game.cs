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
    }

    void OnClick(int i, int j, ClickAction action)
    {
        Debug.Log($"Click ({i}, {j})");

        Cell cell = this.board.At(i, j);
        if (cell.firing)
        {
            return;
        }
        // if (cell.previewing)
        // {

        // }
        if (cell.rotating)
        {
            cell.FinishRotate();
        }

        cell.Rotate(action == ClickAction.RotateCW ? RotateDir.CW : RotateDir.CCW, this.OnCellRotateFinish);
        this.SetDirty();
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

        BoardData boardData = this.gameData.boardData;

        var frees = new List<CellData>();
        foreach (Vector2Int pos in poses)
        {
            frees.Add(boardData.Take(pos.x, pos.y));
        }

        int freeIndex = 0;

        for (int i = 0; i < boardData.width; i++)
        {
            for (int j = 0; j < boardData.height; j++)
            {
                if (boardData.At(i, j) == null)
                {
                    bool found = false;
                    for (int j2 = j + 1; j2 < boardData.height; j2++)
                    {
                        // (i, j2) -> (i, j)
                        if (boardData.At(i, j2) != null)
                        {
                            boardData.Put(i, j, boardData.Take(i, j2));
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Debug.Assert(frees.Count > 0);
                        CellData free = frees[frees.Count - 1];
                        frees.RemoveAt(frees.Count - 1);
                        boardData.Put(i, j, free);
                    }
                }
            }
        }
    }
}