using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance;

    [Header("UI")]
    public GameObject comboPanel;
    public Image[] comboSlots;

    [Header("Colors")]
    public Color normalColor;
    public Color activeColor;

    [Header("Settings")]
    public float comboDuration = 3f;

    int comboCount = 0;
    int consecutiveSales = 0;
    Coroutine comboTimer;

    int totalComboBonus = 0; // ?? LEVEL BOYUNCA KAZANILAN TOPLAM BONUS

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ResetComboVisual();
        totalComboBonus = 0;
    }

    // ?? HER SATIÞTA ÇAÐRILIR
    public void OnSale()
    {
        consecutiveSales++;

        // ilk satýþ ? combo yok
        if (consecutiveSales == 1)
            return;

        // ikinci satýþ ? combo baþlar
        if (consecutiveSales == 2)
        {
            comboPanel.SetActive(true);
            comboCount = 1;
        }
        else
        {
            comboCount++;
        }

        comboCount = Mathf.Clamp(comboCount, 1, comboSlots.Length);
        UpdateComboUI();

        if (comboTimer != null)
            StopCoroutine(comboTimer);

        comboTimer = StartCoroutine(ComboCountdown());
    }

    IEnumerator ComboCountdown()
    {
        yield return new WaitForSeconds(comboDuration);

        GiveBonus();
        ResetCombo();
    }

    void GiveBonus()
    {
        int bonus = 0;

        if (comboCount >= 4)
            bonus = 15;
        else if (comboCount == 3)
            bonus = 10;
        else if (comboCount == 2)
            bonus = 5;

        if (bonus > 0)
        {
            // ?? TOPLAM BONUS BURADA BÝRÝKÝR
            totalComboBonus += bonus;

            // ?? OYUN SIRASINDA PARA HEMEN ARTSIN (ESKÝSÝ GÝBÝ)
            MoneyManager.Instance.currentMoney += bonus;
            MoneyManager.Instance.SendMessage(
                "UpdateUI",
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    void UpdateComboUI()
    {
        for (int i = 0; i < comboSlots.Length; i++)
            comboSlots[i].color = i < comboCount ? activeColor : normalColor;
    }

    void ResetCombo()
    {
        comboCount = 0;
        consecutiveSales = 0;

        ResetComboVisual();
        comboPanel.SetActive(false);
    }

    void ResetComboVisual()
    {
        for (int i = 0; i < comboSlots.Length; i++)
            comboSlots[i].color = normalColor;
    }

    // ?? LevelManager BURADAN OKUYACAK
    public int GetTotalComboBonus()
    {
        return totalComboBonus;
    }
}
