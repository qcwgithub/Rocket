using System.Collections.Generic;

public class GameData
{
    public BoardData boardData;
    public System.Random random;
    public List<RocketData> rocketDatas = new List<RocketData>();
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

        this.rocketDatas.Clear();
        for (int y = 0; y < levelConfig.height; y++)
        {
            RocketData rocketData = new RocketData();
            rocketData.level = this.RandomRocketLevel();
            this.rocketDatas.Add(rocketData);
        }

        // Alg.RefreshLink(boardData);
        this.RefreshLink();
    }

    public int RandomRocketLevel()
    {
        return 1;
    }

    public Shape RandomShape()
    {
        return ShapeExt.Without1()[this.random.Next(0, ShapeExt.Without1().Length)];
        // return Shape.LRTB;
    }

    public void RefreshLink()
    {
        Alg.RefreshLink(this.boardData);
    }
}