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

    void Start()
    {
        barRoot.SetActive(false);     // BAÞTA GÖZÜKMESÝN
        isWaiting = false;            // BAÞTA ÇALIÞMASIN
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
