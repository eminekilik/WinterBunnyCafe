using UnityEngine;

public class CupSlot : MonoBehaviour
{
    public Transform cupPoint;
    public bool isOccupied;

    Cup currentCup;

    public void PlaceCup(GameObject cupObj)
    {
        isOccupied = true;
        cupObj.transform.position = cupPoint.position;

        currentCup = cupObj.GetComponent<Cup>();
        currentCup.currentSlot = this; // ?? kritik satýr
    }

    public Cup GetCup()
    {
        return currentCup;
    }

    public void Clear()
    {
        isOccupied = false;
        currentCup = null;
    }
}
