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
        // success panel
    }

    void LevelFail()
    {
        Debug.Log("Bölüm Baþarýsýz!");
        // fail panel
    }
}
