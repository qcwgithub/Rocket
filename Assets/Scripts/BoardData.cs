using System.Collections.Generic;
using UnityEngine;

public class BoardData
{
    public int width { get; set; }
    public int height { get; set; }
    public CellData[,] cells;
    public List<PreviewGroupData> previewGroupDatas = new List<PreviewGroupData>();

    public void Init(int width, int height)
    {
        this.width = width;
        this.height = height;

        this.cells = new CellData[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                this.cells[x, y] = new CellData();
            }
        }
    }

    public CellData At(int x, int y)
    {
        return this.cells[x, y];
    }

    public void Swap(int fromX, int fromY, int toX, int toY)
    {
        CellData from = this.cells[fromX, fromY];
        Debug.Assert(from != null);

        CellData to = this.cells[toX, toY];
        Debug.Assert(to != null);

        this.cells[fromX, fromY] = to;
        this.cells[toX, toY] = from;
    }

    public PreviewGroupData currentPreviewGroupData
    {
        get
        {
            return this.previewGroupDatas[this.previewGroupDatas.Count - 1];
        }
    }
}