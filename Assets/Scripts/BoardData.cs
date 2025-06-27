public class BoardData : IBoard
{
    public int width{ get; set; }
    public int height{ get; set; }
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

    ICell IBoard.At(int x, int y)
    {
        return this.cells[x, y];
    }

    public CellData At(int x, int y)
    {
        return this.cells[x, y];
    }
}