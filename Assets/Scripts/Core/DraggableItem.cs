using System.Collections;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    [Header("Return Settings")]
    [SerializeField] float validDropDelay = 0.4f;
    [SerializeField] float returnDuration = 0.5f;

    private Vector3 startPosition;
    private bool isDragging;
    private bool addedToContainer;
    private Camera cam;
    private Coroutine returnRoutine;
    public bool IsDragging => isDragging;

    private SpriteRenderer spriteRenderer;
    private int originalSortingOrder;

    void Start()
    {
        cam = Camera.main;
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSortingOrder = spriteRenderer.sortingOrder;
    }

    void Update()
    {
        if (!isDragging) return;

        Vector3 pointerPos = GetPointerWorldPosition();
        transform.position = pointerPos;
    }

    void OnMouseDown()
    {
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }

        isDragging = true;
        addedToContainer = false;
        spriteRenderer.sortingOrder = 100; // öne al
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (addedToContainer)
        {
            StartReturn(validDropDelay);
        }
        else
        {
            StartReturn(0f); // bekleme yok
        }
    }

    // Pot / Cup çaðýrýr
    public void OnAddedToContainer()
    {
        addedToContainer = true;
    }

    void StartReturn(float delay)
    {
        if (returnRoutine != null)
            StopCoroutine(returnRoutine);

        returnRoutine = StartCoroutine(ReturnRoutine(delay));
    }

    IEnumerator ReturnRoutine(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / returnDuration;
            transform.position = Vector3.Lerp(start, startPosition, t);
            yield return null;
        }

        transform.position = startPosition;
        spriteRenderer.sortingOrder = originalSortingOrder;
        returnRoutine = null;
    }

    Vector3 GetPointerWorldPosition()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        Vector3 screenPos = Input.mousePosition;
#else
        if (Input.touchCount == 0) return transform.position;
        Vector3 screenPos = Input.GetTouch(0).position;
#endif
        screenPos.z = Mathf.Abs(cam.transform.position.z);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = transform.position.z;
        return worldPos;
    }
}
