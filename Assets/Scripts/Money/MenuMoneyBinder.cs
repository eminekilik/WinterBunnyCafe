using UnityEngine;
using TMPro;

public class MenuMoneyBinder : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<TMP_Text>().text =
            CurrencyManager.Instance.totalMoney.ToString();
    }
}
