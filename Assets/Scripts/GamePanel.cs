using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    void Start()
    {
        this.OnClickStart();
    }

    public void OnClickStart()
    {
        var random = new System.Random(1);

        LevelConfig levelConfig = sc.configManager.GetLevelConfig(1);
        var boardData = new BoardData(levelConfig.width, levelConfig.height);

        for (int x = 0; x < levelConfig.width; x++)
        {
            for (int y = 0; y < levelConfig.height; y++)
            {
                CellData cell = boardData.At(x, y);
                cell.shape = (Shape)random.Next(0, (int)Shape.Count);
            }
        }

        sc.board.Init(boardData);
    }
}
