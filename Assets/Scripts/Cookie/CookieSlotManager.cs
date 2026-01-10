using UnityEngine;

public class CookieSlotManager : MonoBehaviour
{
    public static CookieSlotManager Instance;

    public CookieSlot[] slots; // SIRALI

    void Awake()
    {
        Instance = this;
    }

    public CookieSlot GetFirstEmptySlot()
    {
        foreach (CookieSlot slot in slots)
        {
            if (!slot.isOccupied)
                return slot;
        }
        return null;
    }
}
