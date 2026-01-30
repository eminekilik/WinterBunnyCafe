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


    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    [Header("Timer FX")]
    public Color warningTimerColor = Color.red;
    public float timerPopScale = 1.3f;
    public float timerPopDuration = 0.15f;

    [Header("Timer Audio")]
    public AudioClip timerTickLoop;

    int lastSecond = -1;
    bool warningStarted;

    Color originalTimerColor;
    Vector3 originalTimerScale;

    [Header("Timer Icon FX")]
    public RectTransform timerIcon;
    public float shakeAmplitude = 3f;   // çok küçük
    public float shakeFrequency = 40f;  // hýzlý titreþim
    public float shakeDuration = 0.18f;

    Vector3 originalIconPos;

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

        originalTimerColor = timerText.color;
        originalTimerScale = timerText.transform.localScale;

        if (timerIcon != null)
            originalIconPos = timerIcon.anchoredPosition;
    }

    void Update()
    {
        if (levelEnded) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (remainingTime <= 0)
        {
            levelEnded = true; // timer dursun
            StopWarning();
            StartCoroutine(EndLevelRoutine());
        }
    }

    void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(Mathf.Max(remainingTime, 0));
        timerText.text = seconds.ToString();

        if (seconds <= 5 && seconds > 0)
        {
            if (!warningStarted)
                StartWarning();

            if (seconds != lastSecond)
            {
                lastSecond = seconds;
                StartCoroutine(TimerPop());

                if (timerIcon != null)
                    StartCoroutine(ShakeIcon());
            }
        }
    }

    IEnumerator ShakeIcon()
    {
        float time = 0f;

        while (time < shakeDuration)
        {
            time += Time.deltaTime;

            // 0-1 arasý yumuþak eðri (baþta ve sonda sakin)
            float strengthMultiplier = Mathf.Sin((time / shakeDuration) * Mathf.PI);

            float x = Random.Range(-2f, 2f) * shakeAmplitude * strengthMultiplier;
            float y = Random.Range(-1f, 1f) * shakeAmplitude * 1f * strengthMultiplier;

            timerIcon.anchoredPosition = originalIconPos + new Vector3(x, y, 0f);

            yield return null;
        }

        timerIcon.anchoredPosition = originalIconPos;
    }

    void StartWarning()
    {
        warningStarted = true;
        timerText.color = warningTimerColor;

        if (audioSource != null && timerTickLoop != null)
        {
            audioSource.clip = timerTickLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void StopWarning()
    {
        timerText.color = originalTimerColor;
        timerText.transform.localScale = originalTimerScale;

        if (audioSource != null && audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }

        if (timerIcon != null)
            timerIcon.anchoredPosition = originalIconPos;
    }

    IEnumerator TimerPop()
    {
        Transform t = timerText.transform;
        Vector3 big = originalTimerScale * timerPopScale;

        float time = 0f;
        while (time < timerPopDuration)
        {
            time += Time.deltaTime;
            t.localScale = Vector3.Lerp(originalTimerScale, big, time / timerPopDuration);
            yield return null;
        }

        time = 0f;
        while (time < timerPopDuration)
        {
            time += Time.deltaTime;
            t.localScale = Vector3.Lerp(big, originalTimerScale, time / timerPopDuration);
            yield return null;
        }
    }

    IEnumerator EndLevelRoutine()
    {
        // coin animasyonlarý bitene kadar bekle
        yield return new WaitUntil(() =>
            MoneyManager.Instance.ActiveCoinCount == 0
        );

        if (MoneyManager.Instance.currentMoney >= targetMoney)
        {
            LevelSuccess();
            if (audioSource != null && winSound != null)
                audioSource.PlayOneShot(winSound);

        }
        else
        {
            LevelFail();
            if (audioSource != null && loseSound != null)
                audioSource.PlayOneShot(loseSound);

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
