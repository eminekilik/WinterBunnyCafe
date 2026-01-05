using UnityEngine;

public class CupArea : MonoBehaviour
{
    public GameObject cupPrefab;
    public CupSlot[] slots; // SIRALI

    void OnMouseDown()
    {
        SpawnCupToFirstEmptySlot();
    }

    void SpawnCupToFirstEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].isOccupied)
            {
                GameObject cup = Instantiate(cupPrefab);
                slots[i].PlaceCup(cup);
                return;
            }
        }

        // boþ slot yoksa
        Debug.Log("Boþ slot yok");
    }
}
