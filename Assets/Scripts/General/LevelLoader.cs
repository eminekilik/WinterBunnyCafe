using UnityEngine;

public static class LevelLoader
{
    public static LevelData SelectedLevel;

    public static LevelData GetNextLevel()
    {
        if (SelectedLevel == null) return null;

        int nextId = SelectedLevel.id + 1;

        LevelData[] allLevels = Resources.LoadAll<LevelData>("Levels");

        foreach (LevelData level in allLevels)
        {
            if (level.id == nextId)
                return level;
        }

        return null; // son level
    }
}
