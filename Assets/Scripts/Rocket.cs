using UnityEngine;

public class Rocket : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    Game game;
    int y;
    public void Init(Game game, int y)
    {
        this.game = game;
        this.y = y;
    }

    void Refresh()
    {
        this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Rocket/rocket0" + this.game.gameData.rocketDatas[this.y].level);
    }
}