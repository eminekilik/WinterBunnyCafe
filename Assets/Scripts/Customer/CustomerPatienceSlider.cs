using UnityEngine;
using UnityEngine.UI;

public class CustomerPatienceSlider : MonoBehaviour
{
    [Header("Patience")]
    public float maxPatience = 10f;
    float currentPatience;

    [Header("UI")]
    public Slider patienceSlider;
    public Image fillImage;
    public GameObject barRoot; // Canvas veya Slider parent

    [Header("Colors")]
    public Color fullColor = Color.green;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;

    bool isWaiting;
    float patienceDecreaseMultiplier = 1f;

    void Start()
    {
        barRoot.SetActive(false);     // BAÞTA GÖZÜKMESÝN
        isWaiting = false;            // BAÞTA ÇALIÞMASIN
        SetPatienceMultiplierFromLevel();
    }

    void Update()
    {
        if (!isWaiting) return;

        currentPatience -= Time.deltaTime;
        patienceSlider.value = currentPatience;

        UpdateColor();

        if (currentPatience <= 0)
        {
            PatienceFinished();
        }
    }

    void SetPatienceMultiplierFromLevel()
    {
        if (LevelLoader.SelectedLevel == null)
        {
            patienceDecreaseMultiplier = 1f;
            return;
        }

        int gameSpeed = LevelLoader.SelectedLevel.gameSpeed;

        switch (gameSpeed)
        {
            case 1: patienceDecreaseMultiplier = 1.0f; break;
            case 2: patienceDecreaseMultiplier = 1.1f; break;
            case 3: patienceDecreaseMultiplier = 1.25f; break;
            case 4: patienceDecreaseMultiplier = 1.4f; break;
            case 5: patienceDecreaseMultiplier = 1.6f; break;
            case 6: patienceDecreaseMultiplier = 1.8f; break;
            default: patienceDecreaseMultiplier = 1f; break;
        }
    }

    public void StartPatience()
    {
        currentPatience = maxPatience;
        patienceSlider.maxValue = maxPatience;
        patienceSlider.value = maxPatience;

        fillImage.color = fullColor;

        barRoot.SetActive(true);
        isWaiting = true;
    }

    public void StopPatience()
    {
        isWaiting = false;
        barRoot.SetActive(false);
    }

    void UpdateColor()
    {
        float t = currentPatience / maxPatience;

        if (t > 0.5f)
            fillImage.color = Color.Lerp(midColor, fullColor, (t - 0.5f) * 2f);
        else
            fillImage.color = Color.Lerp(lowColor, midColor, t * 2f);
    }

    void PatienceFinished()
    {
        StopPatience();
        GetComponent<Customer>().LeaveBecauseOfAnger();
    }
}
