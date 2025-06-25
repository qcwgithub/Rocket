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
        Board board = new Board(levelConfig.width, levelConfig.height);

        for (int x = 0; x < levelConfig.width; x++)
        {
            for (int y = 0; y < levelConfig.height; y++)
            {
                Cell cell = board.At(x, y);
                cell.x = x;
                cell.y = y;
                cell.shape = (Shape)random.Next(0, (int)Shape.Count);
            }
        }

        sc.bootstrap.board.Init(board);
    }
}
