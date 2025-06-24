public class Board
{
    public readonly int width;
    public readonly int height;
    public readonly Cell[,] cells;

    public Board(int width, int height)
    {
        this.width = width;
        this.height = height;

        this.cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                this.cells[x, y] = new Cell();
            }
        }
    }

    public Cell At(int x, int y)
    {
        return this.cells[x, y];
    }
}