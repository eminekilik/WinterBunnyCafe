using UnityEngine;

public class CookieSlot : MonoBehaviour
{
    public Transform cookiePoint;
    public bool isOccupied;

    Cookie currentCookie;

    public void PlaceCookie(GameObject cookieObj)
    {
        isOccupied = true;
        cookieObj.transform.position = cookiePoint.position;

        currentCookie = cookieObj.GetComponent<Cookie>();
        currentCookie.currentSlot = this; // KRÝTÝK
    }

    public void Clear()
    {
        isOccupied = false;
        currentCookie = null;
    }
}
