using UnityEngine;

public class CupSlot : MonoBehaviour
{
    public Transform cupPoint;
    public bool isOccupied;

    CupNew currentCup;

    public void PlaceCup(GameObject cupObj)
    {
        isOccupied = true;
        cupObj.transform.position = cupPoint.position;

        currentCup = cupObj.GetComponent<CupNew>();
    }

    public CupNew GetCup()
    {
        return currentCup;
    }

    public void Clear()
    {
        isOccupied = false;
        currentCup = null;
    }
}
