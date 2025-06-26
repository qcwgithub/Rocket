using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour
{
    public Cell cellTemplate;
    // List<CCell> children = new List<CCell>();
    Cell[,] cells;

    public Cell At(int x, int y)
    {
        return this.cells[x, y];
    }

    public BoardData boardData;
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
                cell.Init(boardData, i, j, CellState.Still);

                cell.transform.position = new Vector3(
                    -boardData.width * 0.5f + 0.5f + i,
                    -boardData.height * 0.5f + 0.5f + j,
                    0f
                );
            }
        }
        this.RefreshColors();
    }

    public void RefreshColors()
    {
        for (int i = 0; i < boardData.width; i++)
        {
            for (int j = 0; j < boardData.height; j++)
            {
                this.At(i, j).ApplyColor();
            }
        }
    }
}