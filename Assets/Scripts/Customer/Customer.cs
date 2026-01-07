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

    [Header("Order Icon")]
    public SpriteRenderer orderIconRenderer;
    public Sprite hotChocolateIcon;
    public Sprite marshmallowHotChocolateIcon;

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

            // ?? sipariþ belirle
            wantedOrder = Random.value > 0.5f
                ? OrderType.HotChocolate
                : OrderType.HotChocolateWithMarshmallow;

            UpdateOrderIcon();

            if (patience != null)
                patience.StartPatience();
        }
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
        }
    }

    public bool CanBeServed()
    {
        return hasArrived && !leaving;
    }

    // ?? Kupayý verince çaðrýlýr
    public void TryServe(Cup cup)
    {
        if (!CanBeServed()) return;

        // ? yanlýþ ürün
        if (cup.GetOrderType() != wantedOrder)
        {
            Debug.Log("Yanlýþ sipariþ!");
            return;
        }

        // ? doðru ürün
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

        // kupayý temizle
        if (cup.currentSlot != null)
            cup.currentSlot.Clear();

        Destroy(cup.gameObject);
    }

    // ?? Sabýr bitince çaðrýlýr
    public void LeaveBecauseOfAnger()
    {
        orderBubble.SetActive(false);

        leaving = true;

        CustomerManager.Instance.RemoveCustomer(this);
        CustomerSlotManager.Instance.FreeSlot(targetSlot);
    }
}
