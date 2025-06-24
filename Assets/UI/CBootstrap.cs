using UnityEngine;

public class CBootstrap : MonoBehaviour
{
    public CGame game;
    void Awake()
    {
        sc.bootstrap = this;

        sc.configManager = new ConfigManager();
        sc.configManager.Load();
    }
}