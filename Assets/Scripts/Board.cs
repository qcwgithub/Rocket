using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IBoard
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
            return this.levelConfig.width;
        }
    }
    public int height
    {
        get
        {
            return this.levelConfig.height;
        }
    }
    ICell IBoard.At(int x, int y)
    {
        return this.cells[x, y];
    }

    public LevelConfig levelConfig;
    public void Init(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;

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
                cell.Init(i, j, CellState.Still, this.levelConfig.RandomNext());

                cell.transform.position = new Vector3(
                    -this.width * 0.5f + 0.5f + i,
                    -this.height * 0.5f + 0.5f + j,
                    0f
                );
            }
        }
        this.RefreshColors();
    }

    public void RefreshColors()
    {
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                this.At(i, j).ApplyColor();
            }
        }
    }
}