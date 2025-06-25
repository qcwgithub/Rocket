using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GamePanel gamePanel;
    public CBoard board;
    void Awake()
    {
        sc.bootstrap = this;

        sc.configManager = new ConfigManager();
        sc.configManager.Load();
    }
}