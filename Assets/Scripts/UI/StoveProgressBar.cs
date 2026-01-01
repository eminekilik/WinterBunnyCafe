using UnityEngine;
using UnityEngine.UI;

public class StoveProgressBar : MonoBehaviour
{
    [SerializeField] Image fillImage;

    [Header("Colors")]
    [SerializeField] Color earlyColor = Color.yellow;
    [SerializeField] Color perfectColor = Color.green;
    [SerializeField] Color overcookColor = Color.red;

    [Header("Smooth")]
    [SerializeField] float smoothSpeed = 6f;

    float targetProgress;
    float currentProgress;

    enum VisualState { Early, Perfect, Overcook }
    VisualState currentState;

    void Update()
    {
        currentProgress = Mathf.Lerp(
            currentProgress,
            targetProgress,
            Time.deltaTime * smoothSpeed
        );

        fillImage.fillAmount = currentProgress;
    }

    public void SetProgress(float progress)
    {
        targetProgress = Mathf.Clamp01(progress);
        UpdateColor(progress);
    }

    void UpdateColor(float progress)
    {
        VisualState newState;

        if (progress < 0.45f)
            newState = VisualState.Early;
        else if (progress <= 0.7f)
            newState = VisualState.Perfect;
        else
            newState = VisualState.Overcook;

        if (newState == currentState) return;

        currentState = newState;

        switch (currentState)
        {
            case VisualState.Early:
                fillImage.color = earlyColor;
                break;
            case VisualState.Perfect:
                fillImage.color = perfectColor;
                break;
            case VisualState.Overcook:
                fillImage.color = overcookColor;
                break;
        }
    }

    public void Show()
    {
        //gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(true);
        currentProgress = 0f;
        targetProgress = 0f;
        fillImage.fillAmount = 0f;
        fillImage.color = earlyColor;
        currentState = VisualState.Early;
    }

    public void Hide()
    {
        //gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
