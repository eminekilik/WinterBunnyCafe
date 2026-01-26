using UnityEngine;

public class ChocolateChipJar : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip chocolateChipSound;

    void OnMouseDown()
    {
        Cup cup =
            CupManager.Instance.GetFirstFilledCupWithCreamWithoutChocolate();

        if (cup == null) return;

        if (audioSource != null && chocolateChipSound != null)
            audioSource.PlayOneShot(chocolateChipSound);

        cup.AddChocolateChips();
    }
}
