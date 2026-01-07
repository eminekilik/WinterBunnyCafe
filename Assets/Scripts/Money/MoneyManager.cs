using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int currentMoney;

    [Header("Prices")]
    public int hotChocolatePrice = 10;
    public int marshmallowHotChocolatePrice = 15;

    [Header("UI")]
    public TextMeshProUGUI moneyText;

    [Header("Coin FX")]
    public GameObject coinPrefab;
    public RectTransform moneyTarget;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddMoneyWithEffect(OrderType orderType, Vector3 worldPos)
    {
        int amount = 0;

        switch (orderType)
        {
            case OrderType.HotChocolate:
                amount = hotChocolatePrice;
                break;

            case OrderType.HotChocolateWithMarshmallow:
                amount = marshmallowHotChocolatePrice;
                break;
        }

        currentMoney += amount;
        UpdateUI();

        GameObject coin = Instantiate(coinPrefab, moneyTarget.parent);
        coin.GetComponent<CoinFly>()
            .StartFly(worldPos, moneyTarget);
    }

    public void OnCoinArrived()
    {
        // Ýstersen ses, scale, glow eklenir
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = currentMoney.ToString();
    }
}
