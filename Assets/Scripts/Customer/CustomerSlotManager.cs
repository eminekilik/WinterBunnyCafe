using System.Collections.Generic;
using UnityEngine;

public class CustomerSlotManager : MonoBehaviour
{
    public static CustomerSlotManager Instance;

    public List<Transform> slots = new List<Transform>();
    private Dictionary<Transform, bool> slotOccupied = new Dictionary<Transform, bool>();

    void Awake()
    {
        Instance = this;

        foreach (var slot in slots)
        {
            slotOccupied[slot] = false;
        }
    }

    public Transform GetRandomFreeSlot()
    {
        List<Transform> freeSlots = new List<Transform>();

        foreach (var kvp in slotOccupied)
        {
            if (!kvp.Value)
                freeSlots.Add(kvp.Key);
        }

        if (freeSlots.Count == 0)
            return null;

        Transform chosen = freeSlots[Random.Range(0, freeSlots.Count)];
        slotOccupied[chosen] = true;
        return chosen;
    }

    public void FreeSlot(Transform slot)
    {
        if (slotOccupied.ContainsKey(slot))
            slotOccupied[slot] = false;
    }
}
