public class BoardData
{
    public int width;
    public int height;
    public CellData[,] cells;

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
}