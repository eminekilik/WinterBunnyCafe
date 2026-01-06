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

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptySprite;

        isFilled = false;
        hasMarshmallow = false;
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
            return OrderType.HotChocolate; // zaten servis edilmez

        if (hasMarshmallow)
            return OrderType.HotChocolateWithMarshmallow;

        return OrderType.HotChocolate;
    }

}
