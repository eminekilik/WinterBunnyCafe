using UnityEngine;

public class CreamJar : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip creamSound;

    void OnMouseDown()
    {
        Cup cup = CupManager.Instance.GetFirstFilledCupWithoutCream();
        if (cup == null) return;

        if (audioSource != null && creamSound != null)
            audioSource.PlayOneShot(creamSound);

        cup.AddCream();
    }
}
