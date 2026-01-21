using UnityEngine;
using TMPro;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public float levelTime = 60f;
    public int targetMoney = 100;

    [Header("UI")]
    public TMP_Text timerText;

    float remainingTime;
    bool levelEnded;

    [Header("End Panels")]
    public GameObject endPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("End Screen")]
    public TextMeshProUGUI loseEndMoneyText;
    public TextMeshProUGUI winBaseMoneyText;
    public TextMeshProUGUI winBonusMoneyText;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (LevelLoader.SelectedLevel != null)
        {
            levelTime = LevelLoader.SelectedLevel.levelTime;
            targetMoney = LevelLoader.SelectedLevel.targetMoney;
        }

        remainingTime = levelTime;
    }

    void Update()
    {
        if (levelEnded) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (remainingTime <= 0)
        {
            levelEnded = true; // timer dursun
            StartCoroutine(EndLevelRoutine());
        }
    }

    void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(Mathf.Max(remainingTime, 0));
        timerText.text = seconds.ToString();
    }

    IEnumerator EndLevelRoutine()
    {
        // coin animasyonlarý bitsin diye bekle
        yield return new WaitForSeconds(2f);

        if (MoneyManager.Instance.currentMoney >= targetMoney)
        {
            LevelSuccess();
        }
        else
        {
            LevelFail();
        }
    }

    void LevelSuccess()
    {
        Debug.Log("Bölüm Baþarýlý!");

        LevelProgressManager.Instance.UnlockLevel(LevelLoader.SelectedLevel.id + 1);

        endPanel.SetActive(true);
        winPanel.SetActive(true);
        losePanel.SetActive(false);

        StartCoroutine(PlayWinMoneySequence());

        Time.timeScale = 0f;

        CurrencyManager.Instance.AddMoney(MoneyManager.Instance.currentMoney);
    }


    void LevelFail()
    {
        Debug.Log("Bölüm Baþarýsýz!");

        loseEndMoneyText.text = MoneyManager.Instance.GetCurrentMoney().ToString();

        endPanel.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(true);

        Time.timeScale = 0f; // oyunu durdurmak istersen

        CurrencyManager.Instance.AddMoney(MoneyManager.Instance.currentMoney);
    }

    IEnumerator PlayWinMoneySequence()
    {
        int target = targetMoney;
        int total = MoneyManager.Instance.GetCurrentMoney();
        int bonus = Mathf.Max(0, total - target);

        // 1?? Baþta sadece hedef para görünsün
        winBaseMoneyText.text = target.ToString();
        winBonusMoneyText.gameObject.SetActive(false);

        // 2?? Kýsa bekleme (ekran otursun)
        yield return new WaitForSecondsRealtime(0.6f);

        if (bonus <= 0)
        {
            winBaseMoneyText.text = total.ToString();
            yield break;
        }

        // 3?? Bonus SONRADAN gelsin
        winBonusMoneyText.gameObject.SetActive(true);
        winBonusMoneyText.text = "+" + bonus;

        // scale reset (önemli)
        winBonusMoneyText.transform.localScale = Vector3.one;

        // 4?? Pop animasyonu
        yield return BonusPop();

        // 5?? Okunmasý için bekle
        yield return new WaitForSecondsRealtime(0.5f);

        // 6?? Kaybol
        yield return BonusFadeOut();

        // 7?? Ana paraya geçmeden mini duraklama
        yield return new WaitForSecondsRealtime(0.2f);

        // 8?? Ana para sayarak artsýn
        yield return BaseMoneyCountUp(target, total);
    }



    IEnumerator BonusPop()
    {
        Transform t = winBonusMoneyText.transform;
        Vector3 start = t.localScale;
        Vector3 big = start * 1.4f;

        float time = 0f;
        while (time < 0.15f)
        {
            time += Time.unscaledDeltaTime;
            t.localScale = Vector3.Lerp(start, big, time / 0.15f);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.2f);
    }

    IEnumerator BonusFadeOut()
    {
        CanvasGroup cg = winBonusMoneyText.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = winBonusMoneyText.gameObject.AddComponent<CanvasGroup>();

        float time = 0f;
        while (time < 0.2f)
        {
            time += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, time / 0.2f);
            yield return null;
        }

        winBonusMoneyText.gameObject.SetActive(false);
        cg.alpha = 1f;
    }

    IEnumerator BaseMoneyCountUp(int from, int to)
    {
        int current = from;

        while (current < to)
        {
            current += Mathf.Max(1, (to - from) / 20);
            current = Mathf.Min(current, to);

            winBaseMoneyText.text = current.ToString();
            yield return new WaitForSecondsRealtime(0.03f);
        }

        // küçük pop
        Transform t = winBaseMoneyText.transform;
        Vector3 start = t.localScale;
        Vector3 big = start * 1.15f;

        float time = 0f;
        while (time < 0.1f)
        {
            time += Time.unscaledDeltaTime;
            t.localScale = Vector3.Lerp(start, big, time / 0.1f);
            yield return null;
        }

        time = 0f;
        while (time < 0.1f)
        {
            time += Time.unscaledDeltaTime;
            t.localScale = Vector3.Lerp(big, start, time / 0.1f);
            yield return null;
        }
    }


}
