using System.Collections;
using UnityEngine;
using TMPro;

public class LevelIntroUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform panel;
    public TMP_Text levelText;

    public float enterDuration = 0.6f;
    public float exitDuration = 0.4f;
    public float stayDuration = 1.2f;

    Vector2 startPos;
    Vector2 targetPos;

    void Start()
    {
        int level = LevelLoader.SelectedLevel.id;
        levelText.text = "LEVEL " + level;

        targetPos = panel.anchoredPosition;
        startPos = targetPos + Vector2.up * 200f;

        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // INIT
        panel.anchoredPosition = startPos;
        panel.localScale = Vector3.one * 0.95f;
        canvasGroup.alpha = 0f;

        // ENTER
        yield return Animate(
            enterDuration,
            t =>
            {
                float ease = EaseOutBack(t);
                panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, ease);
                panel.localScale = Vector3.Lerp(Vector3.one * 0.95f, Vector3.one, ease);
                canvasGroup.alpha = t;
            }
        );

        yield return new WaitForSeconds(stayDuration);

        // EXIT
        yield return Animate(
            exitDuration,
            t =>
            {
                float ease = EaseIn(t);
                panel.anchoredPosition = Vector2.Lerp(targetPos, targetPos + Vector2.down * 100f, ease);
                panel.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.05f, ease);
                canvasGroup.alpha = 1f - t;
            }
        );

        gameObject.SetActive(false);
    }

    IEnumerator Animate(float duration, System.Action<float> onUpdate)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            onUpdate(Mathf.Clamp01(t / duration));
            yield return null;
        }
    }

    // ===== EASING =====
    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }

    float EaseIn(float t)
    {
        return t * t;
    }
}
