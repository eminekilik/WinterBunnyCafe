using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int currentMoney;

    int pendingMoney; // animasyonu bekleyen para

    [Header("Prices")]
    public int hotChocolatePrice = 10;
    public int marshmallowHotChocolatePrice = 15;
    public int creamHotChocolatePrice = 18;
    public int creamChocolateHotChocolatePrice = 22;

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
        int moneyToAdd = 0;

        switch (orderType)
        {
            case OrderType.HotChocolate:
                moneyToAdd = hotChocolatePrice;
                break;

            case OrderType.HotChocolateWithMarshmallow:
                moneyToAdd = marshmallowHotChocolatePrice;
                break;

            case OrderType.HotChocolateWithCream:
                moneyToAdd = creamHotChocolatePrice;
                break;

            case OrderType.HotChocolateWithCreamAndChocolate:
                moneyToAdd = creamChocolateHotChocolatePrice;
                break;
        }

        GameObject coin = Instantiate(coinPrefab, moneyTarget.parent);
        coin.GetComponent<CoinFly>()
            .StartFly(worldPos, moneyTarget, moneyToAdd);
    }

    public void OnCoinArrived(int amount)
    {
        currentMoney += amount;
        UpdateUI();
        StartCoroutine(MoneyPunch());
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = currentMoney.ToString();
    }

    IEnumerator MoneyPunch()
    {
        Transform t = moneyText.transform;
        Vector3 startScale = t.localScale;
        Vector3 bigScale = startScale * 1.15f;

        float time = 0f;

        // büyü
        while (time < 0.08f)
        {
            time += Time.deltaTime;
            t.localScale = Vector3.Lerp(startScale, bigScale, time / 0.08f);
            yield return null;
        }

        time = 0f;
        // geri dön
        while (time < 0.12f)
        {
            time += Time.deltaTime;
            t.localScale = Vector3.Lerp(bigScale, startScale, time / 0.12f);
            yield return null;
        }
    }
}
