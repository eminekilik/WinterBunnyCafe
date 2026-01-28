using UnityEngine;

public class MarketUI : MonoBehaviour
{
    public GameObject marketPanel;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonSound;

    void Start()
    {
        marketPanel.SetActive(false); // oyun baslayinca kapali olsun
    }

    public void OpenMarket()
    {
        marketPanel.SetActive(true);
        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }

    public void CloseMarket()
    {
        marketPanel.SetActive(false);
        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);
    }
}
