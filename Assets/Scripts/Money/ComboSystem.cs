using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance;

    [Header("UI")]
    public GameObject comboPanel;
    public Image[] comboSlots; // 4 kare

    [Header("Colors")]
    public Color normalColor;
    public Color activeColor;

    [Header("Settings")]
    public float comboDuration = 3f;

    int currentCombo = 0;          // panel açýldýktan sonraki combo
    int consecutiveSales = 0;      // peþ peþe satýþ sayýsý

    Coroutine comboTimer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ResetCombo();
    }

    // Para gerçekten eklendiðinde çaðrýlýr
    public void OnSale()
    {
        // süreyi resetle
        if (comboTimer != null)
            StopCoroutine(comboTimer);

        consecutiveSales++;

        // ? Ýlk satýþ: panel yok, combo yok
        if (consecutiveSales == 1)
        {
            comboTimer = StartCoroutine(ComboCountdown());
            return;
        }

        // ? Ýkinci satýþtan itibaren
        if (!comboPanel.activeSelf)
            comboPanel.SetActive(true);

        currentCombo++;
        currentCombo = Mathf.Clamp(currentCombo, 1, comboSlots.Length);

        UpdateComboUI();

        comboTimer = StartCoroutine(ComboCountdown());
    }

    IEnumerator ComboCountdown()
    {
        yield return new WaitForSeconds(comboDuration);

        // ?? combo zinciri bitti ? bonusu ver
        GiveBonus();

        ResetCombo();
    }

    void UpdateComboUI()
    {
        for (int i = 0; i < comboSlots.Length; i++)
        {
            comboSlots[i].color = i < currentCombo ? activeColor : normalColor;
        }
    }

    void GiveBonus()
    {
        int bonus = 0;

        if (currentCombo >= 4)
            bonus = 15;
        else if (currentCombo >= 3)
            bonus = 10;
        else if (currentCombo >= 2)
            bonus = 5;

        if (bonus > 0)
        {
            MoneyManager.Instance.currentMoney += bonus;

            MoneyManager.Instance.SendMessage(
                "UpdateUI",
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    void ResetCombo()
    {
        currentCombo = 0;
        consecutiveSales = 0;

        for (int i = 0; i < comboSlots.Length; i++)
        {
            comboSlots[i].color = normalColor;
        }

        comboPanel.SetActive(false);
    }
}
