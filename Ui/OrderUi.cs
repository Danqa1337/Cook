using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUi : MonoBehaviour
{
    [SerializeField] private Button _cancelButton;

    private void OnEnable()
    {
        OrdersHandler.OnOrderSubmited += OnOrderStarted;
        OrdersHandler.OnOrderCanceled += OnOrderEnded;
        OrdersHandler.OnOrderCompleted += OnOrderEnded;
    }

    private void OnDisable()
    {
        OrdersHandler.OnOrderSubmited -= OnOrderStarted;
        OrdersHandler.OnOrderCanceled -= OnOrderEnded;
        OrdersHandler.OnOrderCompleted -= OnOrderEnded;
    }

    private void Start()
    {
        _cancelButton.gameObject.SetActive(false);
    }

    private void OnOrderEnded(Order order)
    {
        _cancelButton.gameObject.SetActive(false);
    }

    private void OnOrderStarted(Order order)
    {
        _cancelButton.gameObject.SetActive(true);
    }

    public void Cancel()
    {
        OrdersHandler.instance.CancelOrder();
    }
}