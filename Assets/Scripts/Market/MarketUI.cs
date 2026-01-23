using UnityEngine;

public class MarketUI : MonoBehaviour
{
    public GameObject marketPanel;

    void Start()
    {
        marketPanel.SetActive(false); // oyun baslayinca kapali olsun
    }

    public void OpenMarket()
    {
        marketPanel.SetActive(true);
    }

    public void CloseMarket()
    {
        marketPanel.SetActive(false);
    }
}
