using UnityEngine;

public class Cup : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite filledSprite;
    public Sprite marshmallowSprite;

    SpriteRenderer sr;

    public bool isFilled;
    public bool hasMarshmallow;

    public CupSlot currentSlot;

    public Sprite creamSprite;
    public Sprite chocolateChipSprite;

    public bool hasCream;
    public bool hasChocolateChips;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptySprite;

        isFilled = false;
        hasMarshmallow = false;
        hasCream = false;
        hasChocolateChips = false;
    }

    public void Fill()
    {
        isFilled = true;
        sr.sprite = filledSprite;
    }

    // ? YENÝ: Marshmallow ekleme
    public void AddMarshmallow()
    {
        if (!isFilled) return;
        if (hasMarshmallow) return;

        hasMarshmallow = true;
        sr.sprite = marshmallowSprite;
    }

    public void AddCream()
    {
        if (!isFilled) return;
        if (hasCream) return;

        hasCream = true;
        sr.sprite = creamSprite;
    }

    public void AddChocolateChips()
    {
        if (!isFilled) return;
        if (!hasCream) return;          // ?? ÞART
        if (hasChocolateChips) return;

        hasChocolateChips = true;
        sr.sprite = chocolateChipSprite;
    }


    void OnMouseDown()
    {
        if (!isFilled) return;

        OrderType cupType = GetOrderType();

        Customer customer =
            CustomerManager.Instance.GetFirstMatchingCustomer(cupType);

        if (customer == null)
        {
            Debug.Log("Bu ürünü isteyen müþteri yok");
            return;
        }

        customer.TryServe(this);
    }


    void OnDestroy()
    {
        if (currentSlot != null)
        {
            currentSlot.Clear();
            currentSlot = null;
        }
    }

    public OrderType GetOrderType()
    {
        if (!isFilled)
            return OrderType.HotChocolate;

        if (hasCream && hasChocolateChips)
            return OrderType.HotChocolateWithCreamAndChocolate;

        if (hasCream)
            return OrderType.HotChocolateWithCream;

        if (hasMarshmallow)
            return OrderType.HotChocolateWithMarshmallow;

        return OrderType.HotChocolate;
    }


}
