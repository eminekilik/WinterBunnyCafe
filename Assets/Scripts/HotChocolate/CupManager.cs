using UnityEngine;

public class CupManager : MonoBehaviour
{
    public static CupManager Instance;
    public CupSlot[] slots; // SIRALI

    void Awake()
    {
        Instance = this;
    }

    public Cup GetFirstEmptyCup()
    {
        foreach (CupSlot slot in slots)
        {
            if (slot.isOccupied)
            {
                Cup cup = slot.GetCup();
                if (cup != null && !cup.isFilled)
                    return cup;
            }
        }
        return null;
    }
}
