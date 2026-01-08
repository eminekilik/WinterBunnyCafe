using UnityEngine;

public class CreamJar : MonoBehaviour
{
    void OnMouseDown()
    {
        Cup cup = CupManager.Instance.GetFirstFilledCupWithoutCream();
        if (cup == null) return;

        cup.AddCream();
    }
}
