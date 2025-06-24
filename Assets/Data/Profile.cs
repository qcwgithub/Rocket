using UnityEngine;

public class Profile
{
    public readonly string userId;
    public Profile(string userId)
    {
        this.userId = userId;
    }

    public void Load()
    {
        _level = PlayerPrefs.GetInt(K_level, 0);
        _bgm = PlayerPrefs.GetInt(K_bgm) == 1;
        _sound = PlayerPrefs.GetInt(K_sound) == 1;
        _vibrate = PlayerPrefs.GetInt(K_vibrate) == 1;
    }

    string K_level
    {
        get
        {
            return $"{this.userId}_level";
        }
    }
    int _level;
    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            if (_level != value)
            {
                _level = value;
                PlayerPrefs.SetInt(K_level, value);
            }
        }
    }

    string K_bgm
    {
        get
        {
            return $"{this.userId}_bgm";
        }
    }
    bool _bgm;
    public bool bgm
    {
        get
        {
            return _bgm;
        }
        set
        {
            if (_bgm != value)
            {
                _bgm = value;
                PlayerPrefs.SetInt(K_bgm, value ? 1 : 0);
            }
        }
    }

    string K_sound
    {
        get
        {
            return $"{this.userId}_sound";
        }
    }
    bool _sound;
    public bool sound
    {
        get
        {
            return _sound;
        }
        set
        {
            if (_sound != value)
            {
                _sound = value;
                PlayerPrefs.SetInt(K_sound, value ? 1 : 0);
            }
        }
    }

    string K_vibrate
    {
        get
        {
            return $"{this.userId}_vibrate";
        }
    }
    bool _vibrate;
    public bool vibrate
    {
        get
        {
            return _vibrate;
        }
        set
        {
            if (_vibrate != value)
            {
                _vibrate = value;
                PlayerPrefs.SetInt(K_vibrate, value ? 1 : 0);
            }
        }
    }
}