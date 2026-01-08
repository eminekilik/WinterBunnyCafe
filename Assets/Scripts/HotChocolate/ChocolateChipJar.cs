using UnityEngine;

public class ChocolateChipJar : MonoBehaviour
{
    void OnMouseDown()
    {
        Cup cup =
            CupManager.Instance.GetFirstFilledCupWithCreamWithoutChocolate();

        if (cup == null) return;

        cup.AddChocolateChips();
    }
}
