using UnityEngine;
using System.Collections;

public class CoinFly : MonoBehaviour
{
    int coinValue;

    public float moveDuration = 0.6f;

    [Header("Pop Effect")]
    public float popScale = 1.3f;
    public float popDuration = 0.25f;

    [Header("Drop Effect")]
    public float dropDistance = 40f;
    public float dropDuration = 0.12f;
    public float dropWaitTime = 0.2f;

    [Header("Arc Movement")]
    public float arcHeight = 80f;

    RectTransform rect;
    Vector3 startPos;
    Vector3 fixedTargetPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void StartFly(
        Vector3 startWorldPos,
        RectTransform target,
        int value
    )
    {
        coinValue = value;

        startPos = Camera.main.WorldToScreenPoint(
            startWorldPos + new Vector3(0f, 0.5f, 0f)
        );

        rect.position = startPos;
        fixedTargetPos = target.position;

        StartCoroutine(FlyRoutine());
    }

    IEnumerator FlyRoutine()
    {
        // POP
        rect.localScale = Vector3.zero;

        float t = 0f;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(0f, popScale, t / popDuration);
            rect.localScale = Vector3.one * s;
            yield return null;
        }

        t = 0f;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(popScale, 1f, t / 0.1f);
            rect.localScale = Vector3.one * s;
            yield return null;
        }

        // DROP
        Vector3 dropTarget = startPos + Vector3.down * dropDistance;

        t = 0f;
        while (t < dropDuration)
        {
            t += Time.deltaTime;
            rect.position = Vector3.Lerp(startPos, dropTarget, t / dropDuration);
            yield return null;
        }

        yield return new WaitForSeconds(dropWaitTime);

        // FLY TO MONEY
        Vector3 flyStartPos = rect.position;

        t = 0f;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float progress = t / moveDuration;

            Vector3 pos = Vector3.Lerp(flyStartPos, fixedTargetPos, progress);
            pos.y += Mathf.Sin(progress * Mathf.PI) * arcHeight;

            rect.position = pos;
            yield return null;
        }

        // HEDEF
        MoneyManager.Instance.OnCoinArrived(coinValue);
        Destroy(gameObject);
    }
}
