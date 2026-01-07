using UnityEngine;
using System.Collections;

public class CoinFly : MonoBehaviour
{
    public float moveDuration = 0.6f;

    RectTransform rect;
    Vector3 startPos;
    Vector3 fixedTargetPos; // ?? KÝLÝTLÝ HEDEF

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void StartFly(Vector3 startWorldPos, RectTransform target)
    {
        startPos = Camera.main.WorldToScreenPoint(startWorldPos);
        rect.position = startPos;

        // ?? hedefi BAÞTA kilitle
        fixedTargetPos = target.position;

        StartCoroutine(FlyRoutine());
    }

    IEnumerator FlyRoutine()
    {
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            rect.position = Vector3.Lerp(startPos, fixedTargetPos, t / moveDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
