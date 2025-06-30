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

    public int width
    {
        get
        {
            return this.boardData.width;
        }
    }
    public int height
    {
        get
        {
            return this.boardData.height;
        }
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

        while (this.transform.childCount < this.width * this.height)
        {
            Cell cell = Instantiate(this.cellTemplate);
            cell.transform.parent = this.transform;
        }

        this.cells = new Cell[this.width, this.height];

        int index = 0;
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                Cell cell = this.cells[i, j] = this.transform.GetChild(index++).GetComponent<Cell>();
                cell.gameObject.SetActive(true);
                // cell.gameObject.name = $"({i},{j})";
                cell.Init(boardData, i, j);

                cell.transform.position = this.GetPosition(i, j);
            }
        }
        // this.RefreshColors();
    }

    // public void RefreshColors()
    // {
    //     for (int i = 0; i < this.width; i++)
    //     {
    //         for (int j = 0; j < this.height; j++)
    //         {
    //             this.At(i, j).ApplyColor();
    //         }
    //     }
    // }

    public Vector3 GetPosition(int i, int j)
    {
        return new Vector3(
            -this.width * 0.5f + 0.5f + i,
            -this.height * 0.5f + 0.5f + j,
            0f
        );
    }

    public void Refresh()
    {
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                this.At(i, j).Refresh();
            }
        }
    }
}