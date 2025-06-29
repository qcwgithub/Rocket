public class GameData
{
    public BoardData boardData;
    public void Init()
    {
        var random = new System.Random(1);

        LevelConfig levelConfig = sc.configManager.GetLevelConfig(1);
        this.boardData = new BoardData();
        boardData.Init(levelConfig.width, levelConfig.height);

        for (int x = 0; x < levelConfig.width; x++)
        {
            for (int y = 0; y < levelConfig.height; y++)
            {
                CellData cell = boardData.At(x, y);
                cell.forbidLink = false;
                cell.shape = ShapeExt.Without1()[random.Next(0, ShapeExt.Without1().Length)];
            }
        }

        // Alg.RefreshLink(boardData);
        this.RefreshLink();
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