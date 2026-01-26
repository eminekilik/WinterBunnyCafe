using UnityEngine;

public class MarshmallowJar : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip marshmallowSound;

    void OnMouseDown()
    {
        Cup cup = CupManager.Instance.GetFirstFilledCupWithoutMarshmallow();
        if (cup == null) return;

        if (audioSource != null && marshmallowSound != null)
            audioSource.PlayOneShot(marshmallowSound);

        cup.AddMarshmallow();
    }
}
