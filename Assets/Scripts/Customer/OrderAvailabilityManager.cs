using System.Collections.Generic;
using UnityEngine;

public class OrderAvailabilityManager : MonoBehaviour
{
    public static OrderAvailabilityManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public List<OrderType> GetAvailableOrders()
    {
        List<OrderType> available = new List<OrderType>();

        // ?? HER ZAMAN VAR
        available.Add(OrderType.HotChocolate);

        // ?? Süslü içecekler
        if (DecorationManager.Instance.HasMarshmallow)
            available.Add(OrderType.HotChocolateWithMarshmallow);

        if (DecorationManager.Instance.HasCream)
            available.Add(OrderType.HotChocolateWithCream);

        if (DecorationManager.Instance.HasChocolate)
            available.Add(OrderType.HotChocolateWithCreamAndChocolate);

        // ?? Kurabiye (fýrýn varsa)
        if (OvenManager.Instance.HasAnyActiveOven())
            available.Add(OrderType.Cookie);

        return available;
    }

    public OrderType GetRandomAvailableOrder()
    {
        var list = GetAvailableOrders();
        return list[Random.Range(0, list.Count)];
    }
}
