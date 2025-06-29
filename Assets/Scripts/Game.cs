using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Board board;
    public GameData gameData;
    public void Init(GameData gameData)
    {
        this.gameData = gameData;
        this.board.Init(gameData.boardData);
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

                    // this.board.OnClick(i, j, ClickAction.RotateCCW);
                    this.OnClick(i, j, clickL ? ClickAction.RotateCCW : ClickAction.RotateCW);
                    // Debug.Log($"({i},{j})");
                    // CellData cell = boardData.At(i, j);
                    // cell.shape = cell.shape.GetSettings().rotateCCW;

                    // sc.board.At(i, j).ApplyShape();
                    // sc.board.RefreshColors();
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

        if (this.dirty)
        {
            this.dirty = false;

            this.gameData.RefreshLink();
            this.board.Apply();

            this.RefreshPreviewOrFireQueue();
        }
    }

    // int rotateFinishDirty = 0;
    bool dirty = true;

    // List<Cell> rotatingCells = new List<Cell>();
    // List<Cell> rotatedCells = new List<Cell>();
    void OnCellRotateFinish(Cell cell, RotateDir rotateDir)
    {
        // this.rotatingCells.Remove(cell);
        // this.rotatedCells.Add(cell);

        cell.ResetRotation();

        CellData cellData = this.gameData.boardData.At(cell.x, cell.y);
        cellData.forbidLink = false;

        Shape pre = cellData.shape;

        cellData.shape = rotateDir == RotateDir.CW
            ? cellData.shape.GetSettings().rotateCW
            : cellData.shape.GetSettings().rotateCCW;

        Debug.Log($"({cell.x}, {cell.y}) Rotate {pre} -> {cellData.shape}");

        this.dirty = true;
        // this.gameData.RefreshLink();

        // this.board.Apply();

        // this.RefreshPreviewOrFireQueue();
    }

    void OnClick(int i, int j, ClickAction action)
    {
        Debug.Log($"Click ({i}, {j})");

        Cell cell = this.board.At(i, j);
        if (cell.rotating)
        {
            // cell.ResetRotation();
            // cell.Apply();
            // this.OnCellRotateFinish(cell, cell.rotateDir);
            // return;
            Debug.Log("Rotate again");
            cell.FinishRotate();
        }

        // this.rotatingCells.Add(cell);
        CellData cellData = this.gameData.boardData.At(i, j);
        cellData.forbidLink = true;
        cell.Rotate(action == ClickAction.RotateCW ? RotateDir.CW : RotateDir.CCW, this.OnCellRotateFinish);
        this.dirty = true;

        // if (cell.state == CellState.Still || cell.state == CellState.Warn)
        // {
        //     // logic
        //     // CellData cellData = this.board.boardData.At(i, j);
        //     // this.gameData.SetShape(i, j, cellData.shape.GetSettings().rotateCCW);

        //     cell.PlayRotateAnimation("ccw");

        //     //
        //     cell.ApplyShape();
        //     cell.ApplyColor();
        // }
    }

    void OnCellPreviewFinish(Cell _cell)
    {
        if (this.previewGroup == null)
        {
            return;
        }

        for (int i = 0; i < this.previewGroup.poses.Count; i++)
        {
            Vector2Int pos = this.previewGroup.poses[i];
            Cell cell = this.board.At(pos.x, pos.y);
            if (cell.previewing)
            {
                return;
            }
        }

        // done preview
        Debug.Log("Done preview");

        // 
        this.previewGroup = null;
    }

    PreviewGroupData previewGroup;
    // List<Cell> firingCells = new List<Cell>();
    void RefreshPreviewOrFireQueue()
    {
        // if (this.firingCells.Count > 0)
        // {
        //     // dont interrupt
        //     return;
        // }

        if (this.previewGroup != null)
        {
            // try to cancel preview state
            if (this.gameData.boardData.previewGroupDatas.Count == 0)
            {
                for (int i = 0; i < this.previewGroup.poses.Count; i++)
                {
                    Vector2Int pos = this.previewGroup.poses[i];
                    Cell cell = this.board.At(pos.x, pos.y);
                    if (cell.previewing)
                    {
                        cell.CancelPreview();
                    }
                }

                this.previewGroup = null;
            }
        }

        if (this.previewGroup != null)
        {
            // already previewing
            return;
        }

        if (this.gameData.boardData.previewGroupDatas.Count == 0)
        {
            return;
        }

        this.previewGroup = this.gameData.boardData.previewGroupDatas[0].Clone();
        for (int i = 0; i < this.previewGroup.poses.Count; i++)
        {
            Vector2Int pos = this.previewGroup.poses[i];
            Cell cell = this.board.At(pos.x, pos.y);
            cell.Preview(this.OnCellPreviewFinish);
        }
        Debug.Log("Start preview");
    }
}