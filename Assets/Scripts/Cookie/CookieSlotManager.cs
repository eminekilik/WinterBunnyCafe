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
            if (slot == null)
                continue;

            // ? KAPALI SLOT = YOK SAY
            if (!slot.gameObject.activeInHierarchy)
                continue;

            if (!slot.isOccupied)
                return slot;
        }

        return null;
    }

}
