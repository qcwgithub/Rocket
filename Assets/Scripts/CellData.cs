public class CellData
{
    public Shape shape; // changable
    public bool yellow;
    public bool red;
    public bool green
    {
        get
        {
            return this.yellow && this.red;
        }
    }
}