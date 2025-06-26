using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour
{
    public BoardData boardData;
    public Cell cellTemplate;
    // List<CCell> children = new List<CCell>();
    Cell[,] cells;

    public Cell At(int x, int y)
    {
        return this.cells[x, y];
    }

    public void Init(BoardData boardData)
    {
        this.boardData = boardData;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }

        while (this.transform.childCount < boardData.width * boardData.height)
        {
            Cell cell = Instantiate(this.cellTemplate);
            cell.transform.parent = this.transform;
        }

        this.cells = new Cell[boardData.width, boardData.height];

        int index = 0;
        for (int i = 0; i < boardData.width; i++)
        {
            for (int j = 0; j < boardData.height; j++)
            {
                Cell cell = this.cells[i, j] = this.transform.GetChild(index++).GetComponent<Cell>();
                cell.gameObject.SetActive(true);
                // cell.gameObject.name = $"({i},{j})";
                cell.Init(boardData.At(i, j));

                cell.transform.position = new Vector3(
                    -boardData.width * 0.5f + 0.5f + i,
                    -boardData.height * 0.5f + 0.5f + j,
                    0f
                );
            }
        }
        this.RefreshColors();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log(mousePos);

            float x = mousePos.x;
            float y = mousePos.y;
            if (x > -this.boardData.width * 0.5f && x < this.boardData.width * 0.5f)
            {
                if (y > -this.boardData.height * 0.5f && y < this.boardData.height * 0.5f)
                {
                    int i = (int)(x - -this.boardData.width * 0.5f);
                    int j = (int)(y - -this.boardData.height * 0.5f);
                    // Debug.Log($"({i},{j})");
                    CellData cell = this.boardData.At(i, j);
                    cell.shape = cell.shape.GetSettings().rotateCCW;
                    this.At(i, j).ApplyShape();
                    this.RefreshColors();
                }
            }
        }
    }

    void RefreshColors()
    {
        Alg.RefreshColor(this.boardData);
        for (int i = 0; i < boardData.width; i++)
        {
            for (int j = 0; j < boardData.height; j++)
            {
                this.At(i, j).ApplyColor();
            }
        }
    }
}