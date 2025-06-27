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
        // var gameData = new GameData();
        // gameData.Init();

        LevelConfig levelConfig = sc.configManager.GetLevelConfig(1);
        sc.game.Init(levelConfig);
    }
}
