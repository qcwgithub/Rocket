public class LevelConfig
{
    public int level;
    public int width;
    public int height;

    System.Random random;
    public Shape RandomNext()
    {
        if (this.random == null)
        {
            this.random = new System.Random(1);
        }
        return (Shape)this.random.Next(0, (int)Shape.Count);
    }
}