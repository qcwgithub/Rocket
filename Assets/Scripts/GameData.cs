public class GameData
{
    public BoardData boardData;
    public System.Random random;
    public void Init()
    {
        this.random = new System.Random(1);

        LevelConfig levelConfig = sc.configManager.GetLevelConfig(1);
        this.boardData = new BoardData();
        boardData.Init(levelConfig.width, levelConfig.height);

        for (int x = 0; x < levelConfig.width; x++)
        {
            for (int y = 0; y < levelConfig.height; y++)
            {
                CellData cell = boardData.At(x, y);
                cell.forbidLink = false;
                cell.shape = this.RandomShape();
            }
        }

        // Alg.RefreshLink(boardData);
        this.RefreshLink();
    }

    public Shape RandomShape()
    {
        return ShapeExt.Without1()[this.random.Next(0, ShapeExt.Without1().Length)];
    }

    public void RefreshLink()
    {
        Alg.RefreshLink(this.boardData);
    }

    // public void SetShape(int x, int y, Shape shape)
    // {
    //     CellData cellData = this.boardData.At(x, y);
    //     cellData.shape = shape;

        

    //     // Alg.RefreshColor(this.boardData);
    // }



    public void Shift()
    {

    }
}