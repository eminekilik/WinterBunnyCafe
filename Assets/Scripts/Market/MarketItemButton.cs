using UnityEngine;

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

    public void Buy()
    {
        if (!CurrencyManager.Instance.SpendMoney(price))
            return;

        string key = GetSaveKey(itemType);

        int current = PlayerPrefs.GetInt(key, 0);
        PlayerPrefs.SetInt(key, current + 1);
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
