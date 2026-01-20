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

    [Header("Cookie Order")]
    public Sprite cookieIcon;

    [Header("Customer Appearance")]
    public SpriteRenderer bodyRenderer;
    public Sprite[] customerSprites;

    static int lastSpriteIndex = -1;
    float moveSpeedMultiplier = 1f;


    void Start()
    {
        SetRandomAppearance();
        // Spawn olduðu yer = çýkýþ noktasý
        exitTarget = transform.position;

        SetMoveSpeedMultiplierFromLevel();

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
                moveSpeed * moveSpeedMultiplier * Time.deltaTime
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
            moveSpeed * moveSpeedMultiplier * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetSlot.position) < 0.05f)
        {
            hasArrived = true;
            orderBubble.SetActive(true);

            //wantedOrder = GetRandomOrder();
            wantedOrder = OrderAvailabilityManager.Instance.GetRandomAvailableOrder();

            UpdateOrderIcon();

            if (patience != null)
                patience.StartPatience();
        }
    }

    void SetMoveSpeedMultiplierFromLevel()
    {
        if (LevelLoader.SelectedLevel == null)
        {
            moveSpeedMultiplier = 1f;
            return;
        }

        int gameSpeed = LevelLoader.SelectedLevel.gameSpeed;

        switch (gameSpeed)
        {
            case 1: moveSpeedMultiplier = 1.0f; break;
            case 2: moveSpeedMultiplier = 1.05f; break;
            case 3: moveSpeedMultiplier = 1.1f; break;
            case 4: moveSpeedMultiplier = 1.2f; break;
            case 5: moveSpeedMultiplier = 1.3f; break;
            case 6: moveSpeedMultiplier = 1.4f; break;
            default: moveSpeedMultiplier = 1f; break;
        }
    }

    void SetRandomAppearance()
    {
        if (bodyRenderer == null) return;
        if (customerSprites == null || customerSprites.Length == 0) return;

        int newIndex;

        if (customerSprites.Length == 1)
        {
            newIndex = 0;
        }
        else
        {
            do
            {
                newIndex = Random.Range(0, customerSprites.Length);
            }
            while (newIndex == lastSpriteIndex);
        }

        lastSpriteIndex = newIndex;
        bodyRenderer.sprite = customerSprites[newIndex];
    }


    //OrderType GetRandomOrder()
    //{
    //    float r = Random.value;

    //    if (r < 0.2f)
    //        return OrderType.Cookie;

    //    if (r < 0.4f)
    //        return OrderType.HotChocolate;

    //    if (r < 0.6f)
    //        return OrderType.HotChocolateWithMarshmallow;

    //    if (r < 0.8f)
    //        return OrderType.HotChocolateWithCream;

    //    return OrderType.HotChocolateWithCreamAndChocolate;
    //}


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

            case OrderType.Cookie:
                orderIconRenderer.sprite = cookieIcon;
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

    public void TryServe(Cookie cookie)
    {
        if (!CanBeServed()) return;

        if (cookie.GetOrderType() != wantedOrder)
        {
            Debug.Log("Yanlýþ sipariþ!");
            return;
        }

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

        if (cookie.currentSlot != null)
            cookie.currentSlot.Clear();

        Destroy(cookie.gameObject);
    }


    public void LeaveBecauseOfAnger()
    {
        orderBubble.SetActive(false);

        leaving = true;

        CustomerManager.Instance.RemoveCustomer(this);
        CustomerSlotManager.Instance.FreeSlot(targetSlot);
    }
}
