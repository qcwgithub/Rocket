using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GamePanel gamePanel;
    public Game game;
    void Awake()
    {
        sc.bootstrap = this;
        sc.game = this.game;

        sc.configManager = new ConfigManager();
        sc.configManager.Load();
    }
}