using UnityEngine;

public class Customer : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject orderBubble;

    Transform targetSlot;
    Vector3 exitTarget;

    bool hasArrived;
    bool leaving;

    void Start()
    {
        // Spawn olduðu yer = çýkýþ noktasý
        exitTarget = transform.position;

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
        // Çýkýþ
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

        // Slotuna yürüyüþ
        if (hasArrived) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetSlot.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetSlot.position) < 0.05f)
        {
            hasArrived = true;
            orderBubble.SetActive(true);
        }
    }

    public bool CanBeServed()
    {
        return hasArrived && !leaving;
    }

    public void ServeAndLeave()
    {
        orderBubble.SetActive(false);
        leaving = true;

        CustomerManager.Instance.RemoveCustomer(this);
        CustomerSlotManager.Instance.FreeSlot(targetSlot);
    }
}
