using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DraggableItem))]
public class DraggableSpriteSwap : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;   // kapalý / default
    [SerializeField] private Sprite draggingSprite; // açýk / dökme hali

    private SpriteRenderer spriteRenderer;
    private DraggableItem draggableItem;

    private bool wasDraggingLastFrame;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        draggableItem = GetComponent<DraggableItem>();

        // güvenlik
        if (normalSprite != null)
            spriteRenderer.sprite = normalSprite;
    }

    void Update()
    {
        bool isDraggingNow = draggableItem.IsDragging;

        //  sürükleme baþladý
        if (isDraggingNow && !wasDraggingLastFrame)
        {
            OnDragStart();
        }

        //  sürükleme bitti
        if (!isDraggingNow && wasDraggingLastFrame)
        {
            OnDragEnd();
        }

        wasDraggingLastFrame = isDraggingNow;
    }

    void OnDragStart()
    {
        if (draggingSprite != null)
            spriteRenderer.sprite = draggingSprite;
    }

    void OnDragEnd()
    {
        if (normalSprite != null)
            spriteRenderer.sprite = normalSprite;
    }
}
