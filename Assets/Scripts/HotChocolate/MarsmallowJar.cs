using UnityEngine;

public class MarshmallowJar : MonoBehaviour
{
    void OnMouseDown()
    {
        Cup cup = CupManager.Instance.GetFirstFilledCupWithoutMarshmallow();
        if (cup == null) return;

        cup.AddMarshmallow();
    }
}
