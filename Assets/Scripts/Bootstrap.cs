using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GamePanel gamePanel;
    public Game game;
    public Board board;
    void Awake()
    {
        sc.bootstrap = this;
        sc.game = this.game;
        sc.board = this.board;

        sc.configManager = new ConfigManager();
        sc.configManager.Load();
    }
}