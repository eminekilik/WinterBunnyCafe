using UnityEngine;

public class Cup : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite filledSprite;

    SpriteRenderer sr;
    public bool isFilled;

    // ?? Kupayý hangi slotta tuttuðumuzu biliyoruz
    public CupSlot currentSlot;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptySprite;
        isFilled = false;
    }

    public void Fill()
    {
        isFilled = true;
        sr.sprite = filledSprite;
    }

    void OnMouseDown()
    {
        if (!isFilled) return;

        Customer customer = CustomerManager.Instance.GetFirstWaitingCustomer();
        if (customer == null) return;

        customer.ServeAndLeave();

        // ? slotu temizle
        if (currentSlot != null)
        {
            currentSlot.Clear();
            currentSlot = null;
        }

        Destroy(gameObject);
    }

    // ekstra güvenlik: baþka yerden Destroy edilirse de slot boþalsýn
    void OnDestroy()
    {
        if (currentSlot != null)
        {
            currentSlot.Clear();
            currentSlot = null;
        }
    }
}
