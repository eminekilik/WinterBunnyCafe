using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int currentMoney;

    int pendingMoney; // (ileride kullanýrsan diye duruyor)

    [Header("Prices")]
    public int hotChocolatePrice = 10;
    public int marshmallowHotChocolatePrice = 15;
    public int creamHotChocolatePrice = 18;
    public int creamChocolateHotChocolatePrice = 22;
    public int cookiePrice = 12;

    [Header("UI")]
    public TextMeshProUGUI moneyText;
    public Slider moneySlider;

    [Header("Coin FX")]
    public GameObject coinPrefab;
    public RectTransform moneyTarget;

    public int ActiveCoinCount { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentMoney = 0;
        StartCoroutine(DelayedUIInit());
    }

    IEnumerator DelayedUIInit()
    {
        // LevelManager Start çalýþsýn diye 1 frame bekle
        yield return null;

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

            case OrderType.Cookie:
                moneyToAdd = cookiePrice;
                break;
        }

        GameObject coin = Instantiate(coinPrefab, moneyTarget.parent);

        ActiveCoinCount++;

        coin.GetComponent<CoinFly>()
            .StartFly(worldPos, moneyTarget, moneyToAdd);
    }

    public void OnCoinArrived(int amount)
    {
        currentMoney += amount;
        UpdateUI();
        StartCoroutine(MoneyPunch());

        ActiveCoinCount = Mathf.Max(0, ActiveCoinCount - 1);
    }

    void UpdateUI()
    {
        if (LevelManager.Instance == null) return;

        int target = LevelManager.Instance.targetMoney;

        // TEXT
        if (moneyText != null)
            moneyText.text = currentMoney + " / " + target;

        // SLIDER
        if (moneySlider != null)
        {
            float progress = (float)currentMoney / target;
            moneySlider.value = Mathf.Clamp01(progress);
        }
    }

    IEnumerator MoneyPunch()
    {
        if (moneyText == null) yield break;

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

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

}
