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
        var gameData = new GameData();
        gameData.Init();

        sc.game.Init(gameData);
    }
}
