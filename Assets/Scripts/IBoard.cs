public interface IBoard
{
    int width{ get; }
    int height { get; }
    ICell At(int x, int y);

}