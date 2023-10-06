using System;
using UnityEngine;

public class OrdersHandler : Singleton<OrdersHandler>
{
    private Order _currentOrder;

    public Order CurrentOrder { get => _currentOrder; }

    public static event Action<Order> OnOrderSubmited;

    public static event Action<Order> OnOrderCompleted;

    public static event Action<Order> OnOrderCanceled;

    public static event Action<Order> OnOrderPreviewStart;

    public static event Action<Order> OnOrderDeclined;

    private void OnEnable()
    {
        Kitchen.OnDishFinalized += OnDishFinalized;
    }

    private void OnDisable()
    {
        Kitchen.OnDishFinalized -= OnDishFinalized;
    }

    private void OnDishFinalized(Dish dish)
    {
        if (_currentOrder != null)
        {
            Debug.Log(_currentOrder + " Completed");
            _currentOrder.Complete(dish);
            OnOrderCompleted?.Invoke(_currentOrder);
            _currentOrder = null;
        }
    }

    public void SubmitOrder(Order order)
    {
        _currentOrder = order;
        Debug.Log(_currentOrder + " Submited");
        OnOrderSubmited?.Invoke(order);
        Kitchen.instance.StartNewDish();
    }

    public void CancelOrder()
    {
        if (_currentOrder != null)
        {
            Debug.Log(_currentOrder + " Canceled");
            OnOrderCanceled?.Invoke(_currentOrder);
            _currentOrder.Cancel();
            _currentOrder = null;
        }
    }

    public void DeclineOrder(Order order)
    {
        OnOrderDeclined?.Invoke(order);
    }

    public void StartOrderPreview(Order order)
    {
        Debug.Log("Order preview started");
        OnOrderPreviewStart?.Invoke(order);
    }
}