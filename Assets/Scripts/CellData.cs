public class CellData : ICell
{
    public Shape shape { get; set; } // changable
    public bool yellow{ get; set; }
    public bool red{ get; set; }
    public bool green
    {
        get
        {
            return this.yellow && this.red;
        }
    }
}