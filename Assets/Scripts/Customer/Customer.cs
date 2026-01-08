using UnityEngine;

public class Customer : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject orderBubble;

    Transform targetSlot;
    Vector3 exitTarget;

    bool hasArrived;
    bool leaving;

    CustomerPatienceSlider patience;

    public OrderType wantedOrder;

    [Header("Order Icons")]
    public SpriteRenderer orderIconRenderer;
    public Sprite hotChocolateIcon;
    public Sprite marshmallowHotChocolateIcon;
    public Sprite creamHotChocolateIcon;
    public Sprite creamChocolateHotChocolateIcon;

    void Start()
    {
        // Spawn olduðu yer = çýkýþ noktasý
        exitTarget = transform.position;

        patience = GetComponent<CustomerPatienceSlider>();

        targetSlot = CustomerSlotManager.Instance.GetRandomFreeSlot();
        if (targetSlot == null)
        {
            Destroy(gameObject);
            return;
        }

        orderBubble.SetActive(false);
        CustomerManager.Instance.RegisterCustomer(this);
    }

    void Update()
    {
        // Çýkýþa doðru yürüme
        if (leaving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                exitTarget,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, exitTarget) < 0.05f)
                Destroy(gameObject);

            return;
        }

        // Slotuna ulaþtýysa bekle
        if (hasArrived) return;

        // Slotuna yürü
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetSlot.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetSlot.position) < 0.05f)
        {
            hasArrived = true;
            orderBubble.SetActive(true);

            wantedOrder = GetRandomOrder();
            UpdateOrderIcon();

            if (patience != null)
                patience.StartPatience();
        }
    }

    OrderType GetRandomOrder()
    {
        float r = Random.value;

        if (r < 0.25f)
            return OrderType.HotChocolate;

        if (r < 0.5f)
            return OrderType.HotChocolateWithMarshmallow;

        if (r < 0.75f)
            return OrderType.HotChocolateWithCream;

        return OrderType.HotChocolateWithCreamAndChocolate;
    }

    void UpdateOrderIcon()
    {
        if (orderIconRenderer == null) return;

        switch (wantedOrder)
        {
            case OrderType.HotChocolate:
                orderIconRenderer.sprite = hotChocolateIcon;
                break;

            case OrderType.HotChocolateWithMarshmallow:
                orderIconRenderer.sprite = marshmallowHotChocolateIcon;
                break;

            case OrderType.HotChocolateWithCream:
                orderIconRenderer.sprite = creamHotChocolateIcon;
                break;

            case OrderType.HotChocolateWithCreamAndChocolate:
                orderIconRenderer.sprite = creamChocolateHotChocolateIcon;
                break;
        }
    }

    public bool CanBeServed()
    {
        return hasArrived && !leaving;
    }

    public void TryServe(Cup cup)
    {
        if (!CanBeServed()) return;

        // yanlýþ ürün
        if (cup.GetOrderType() != wantedOrder)
        {
            Debug.Log("Yanlýþ sipariþ!");
            return;
        }

        // doðru ürün
        orderBubble.SetActive(false);

        MoneyManager.Instance.AddMoneyWithEffect(
            wantedOrder,
            transform.position
        );

        if (patience != null)
            patience.StopPatience();

        leaving = true;

        CustomerManager.Instance.RemoveCustomer(this);
        CustomerSlotManager.Instance.FreeSlot(targetSlot);

        if (cup.currentSlot != null)
            cup.currentSlot.Clear();

        Destroy(cup.gameObject);
    }

    public void LeaveBecauseOfAnger()
    {
        orderBubble.SetActive(false);

        leaving = true;

        CustomerManager.Instance.RemoveCustomer(this);
        CustomerSlotManager.Instance.FreeSlot(targetSlot);
    }
}
