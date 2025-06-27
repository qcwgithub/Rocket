public class CellData
{
    public Shape shape;
    public bool forbidLink;
    public bool linkedL;
    public bool linkedR;
    public bool linkedLR
    {
        get
        {
            return this.linkedL && this.linkedR;
        }
    }
}