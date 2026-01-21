using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        if (levelIndex == 1) return true; // 1. level her zaman açýk

        return PlayerPrefs.GetInt("Level_" + levelIndex, 0) == 1;
    }

    public void UnlockLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("Level_" + levelIndex, 1);
        PlayerPrefs.Save();
    }
}
