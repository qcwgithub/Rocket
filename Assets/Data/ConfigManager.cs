using System.Collections.Generic;
using UnityEngine;

public class ConfigManager
{
    public List<LevelConfig> levelConfigs;

    public int maxLevel;

    public void Load()
    {
        this.levelConfigs = new List<LevelConfig>();

        this.levelConfigs.Add(new LevelConfig
        {
            level = 1,
            width = 5,
            height = 5,
        });

        this.maxLevel = this.levelConfigs[this.levelConfigs.Count - 1].level;

        // int level = 1;
        // while (true)
        // {
        //     TextAsset textAsset = Resources.Load<TextAsset>("Level/" + level);
        //     if (textAsset == null)
        //     {
        //         break;
        //     }

        //     // LevelConfig levelConfig = 
        // }
    }

    public LevelConfig GetLevelConfig(int level)
    {
        int index = level - 1;
        if (index >= 0 && index < this.levelConfigs.Count)
        {
            LevelConfig levelConfig = this.levelConfigs[index];
            UnityEngine.Debug.Assert(levelConfig.level == level);
            return levelConfig;
        }
        return null;
    }
}