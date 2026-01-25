using UnityEngine;
using TMPro;

public enum ShopItemType
{
    Oven,
    Pot,
    CupSlot,
    CookieSlot
}

public class MarketItemButton : MonoBehaviour
{
    public int price;
    public ShopItemType itemType;

    [Header("UI")]
    public TMP_Text priceText;

    int basePrice;

    void Start()
    {
        basePrice = price;
        RestoreState();
    }

    void RestoreState()
    {
        // Level 4 kontrolü (fýrýn & kurabiye slotu için)
        if ((itemType == ShopItemType.Oven || itemType == ShopItemType.CookieSlot) &&
            !LevelProgressManager.Instance.IsLevelUnlocked(4))
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        string key = GetSaveKey(itemType);
        int current = PlayerPrefs.GetInt(key, 0);

        // 1 tane alýnmýþsa fiyat arttýrýlmýþ haliyle gelsin
        if (current == 1)
        {
            price = basePrice + 100;
        }

        // 2 tane alýnmýþsa marketten tamamen kaldýr
        if (current >= 2)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        UpdatePriceText();
    }


    public void Buy()
    {
        string key = GetSaveKey(itemType);
        int current = PlayerPrefs.GetInt(key, 0);

        if (current >= 2)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        if (!CurrencyManager.Instance.SpendMoney(price))
            return;

        current++;
        PlayerPrefs.SetInt(key, current);

        if (current == 1)
        {
            price = basePrice + 100;
            UpdatePriceText();
        }

        if (current >= 2)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    void UpdatePriceText()
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    string GetSaveKey(ShopItemType type)
    {
        switch (type)
        {
            case ShopItemType.Oven: return "ShopOvenCount";
            case ShopItemType.Pot: return "ShopPotCount";
            case ShopItemType.CupSlot: return "ShopCupSlotCount";
            case ShopItemType.CookieSlot: return "ShopCookieSlotCount";
        }
        return "";
    }
}
