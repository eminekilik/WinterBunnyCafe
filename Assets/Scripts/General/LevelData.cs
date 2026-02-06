using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Data")]
public class LevelData : ScriptableObject
{
    public int id;
    public float levelTime;
    public int targetMoney;
    public int gameSpeed;

    [Header("Restrictions")]
    public LevelRestriction[] restrictions;
}

public enum LevelRestriction
{
    NoTrash,                 // çöp atmak yasak
    NoUnhappyCustomer        // memnuniyetsiz müþteri gönderme
}