using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    List<Customer> waitingCustomers = new List<Customer>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterCustomer(Customer customer)
    {
        if (!waitingCustomers.Contains(customer))
            waitingCustomers.Add(customer);
    }

    public void RemoveCustomer(Customer customer)
    {
        waitingCustomers.Remove(customer);
    }

    public Customer GetFirstWaitingCustomer()
    {
        foreach (var customer in waitingCustomers)
        {
            if (customer != null && customer.CanBeServed())
                return customer;
        }

        return null;
    }

    // ?? ASIL ÖNEMLÝ OLAN
    public Customer GetFirstMatchingCustomer(OrderType orderType)
    {
        foreach (Customer customer in waitingCustomers)
        {
            if (customer == null) continue;
            if (!customer.CanBeServed()) continue;

            if (customer.wantedOrder == orderType)
                return customer;
        }

        return null;
    }
}
