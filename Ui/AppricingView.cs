using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppricingView : UiCanvas
{
    [SerializeField] private Label _scorelabel;
    [SerializeField] private AppriceResultIcon _appriceResultIcon;

    private void OnEnable()
    {
        OrdersHandler.OnOrderSubmited += OnOrderSubmited;
    }

    private void OnDisable()
    {
        OrdersHandler.OnOrderSubmited -= OnOrderSubmited;
    }

    private void OnOrderSubmited(Order order)
    {
        order.OnAppriced += OnAppriced;
    }

    private void OnAppriced(AppriceResult result)
    {
        _appriceResultIcon.Draw(result);
        _scorelabel.SetText("Блюдо готово!");
        Show();
        DataHolder.instance.Save();
    }

    public void Submit()
    {
        GameStateManager.ChangeGameState(GameState.Cooking);
    }
}