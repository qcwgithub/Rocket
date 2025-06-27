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

        float dt = Time.deltaTime;

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                Cell cell = board.At(i, j);
                cell.MyUpdate(dt);
            }
        }

        //
    }

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

        this.gameData.RefreshLink();

        // cell.Apply();
        this.board.Apply();

        this.RefreshPreviewOrFireQueue();
    }

    void OnClick(int i, int j, ClickAction action)
    {
        Debug.Log($"Click ({i}, {j})");

        Cell cell = this.board.At(i, j);
        if (cell.rotating)
        {
            return;
        }

        // this.rotatingCells.Add(cell);
        CellData cellData = this.gameData.boardData.At(i, j);
        cellData.forbidLink = true;
        cell.Rotate(action == ClickAction.RotateCW ? RotateDir.CW : RotateDir.CCW, this.OnCellRotateFinish);

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

    List<Cell> previewingCells = new List<Cell>();
    List<Cell> firingCells = new List<Cell>();
    void RefreshPreviewOrFireQueue()
    {
        if (this.firingCells.Count > 0)
        {
            // dont interrupt
            return;
        }

        if (this.previewingCells.Count > 0)
        {
            // try to cancel preview state
        }

        if (this.previewingCells.Count > 0)
        {
            // already previewing
            return;
        }

        int firstX = -1;
        int firstY = -1;
        for (int j = board.height - 1; j >= 0; j--)
        {
            for (int i = 0; i < board.width; i++)
            {
                CellData cellData = this.gameData.boardData.At(i, j);
                if (cellData.linkedLR)
                {
                    firstX = i;
                    firstY = j;
                    break;
                }
            }

            if (firstX != -1)
            {
                break;
            }
        }

        if (firstX == -1)
        {
            return;
        }

        
    }
}