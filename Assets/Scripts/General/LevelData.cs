using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Data")]
public class LevelData : ScriptableObject
{
    public int id;
    public float levelTime;
    public int targetMoney;
}
