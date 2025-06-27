using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Board board;
    public LevelConfig levelConfig;
    public void Init(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;
        this.board.Init(levelConfig);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                    this.OnClick(i, j, ClickAction.RotateCCW);
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

    List<Cell> rotatingCells = new List<Cell>();
    void OnCellRotateFinish(Cell cell)
    {
        this.rotatingCells.Remove(cell);
    }

    void OnClick(int i, int j, ClickAction action)
    {
        Cell cell = this.board.At(i, j);
        if (cell.rotating)
        {
            return;
        }

        this.rotatingCells.Add(cell);
        cell.Rotate("ccw", this.OnCellRotateFinish);

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

    void Event_CellRotateFinish(int i, int j)
    {

    }

    void Event_CellDropFinish()
    {

    }
}