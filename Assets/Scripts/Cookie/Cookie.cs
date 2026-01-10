using UnityEngine;

public class Cookie : MonoBehaviour
{
    public CookieSlot currentSlot;

    public OrderType GetOrderType()
    {
        return OrderType.Cookie;
    }

    void OnMouseDown()
    {
        Customer customer =
            CustomerManager.Instance.GetFirstMatchingCustomer(GetOrderType());

        if (customer == null)
        {
            Debug.Log("Kurabiye isteyen müþteri yok");
            return;
        }

        customer.TryServe(this);
    }
}
