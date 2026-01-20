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
            if (!slots[i].isOccupied && slots[i].gameObject.activeInHierarchy)
            {
                GameObject cup = Instantiate(cupPrefab);
                slots[i].PlaceCup(cup);
                return;
            }
        }

        Debug.Log("Aktif ve boþ slot yok");
    }

}
