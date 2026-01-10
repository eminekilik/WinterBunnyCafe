using UnityEngine;

public class Dough : MonoBehaviour
{
    void OnMouseDown()
    {
        OvenCooking oven = OvenManager.Instance.GetFirstEmptyOven();
        if (oven == null) return;

        oven.StartCooking();
    }
}
