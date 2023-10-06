using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : Person
{
    [SerializeField] private Order _order;
    [SerializeField] private int _minOrderRad = 5;
    [SerializeField] private int _maxOrderRad = 7;

    public Order Order { get => _order; }

    public override PersonType PersonType => PersonType.Customer;

    public override string Name => "Заказчик";

    protected override void Awake()
    {
        base.Awake();
        _mapObject.Hide();
    }

    private void OnOrderCompleted(Dish dish)
    {
        transform.SetParent(null);
        var stagesMult = MathF.Max(1, (float)_order.InterStages.Where(v => v.reached).ToList().Count / (float)_order.InterStages.Count);
        var finalValue = (int)((Math.Max(1, dish.Score) * _order.BaseReward) * stagesMult);

        var interStagesReached = _order.InterStages.Count == 0 ? 1 : (float)_order.InterStages.Where(v => v.reached).ToList().Count / (float)_order.InterStages.Count;
        Debug.Log(interStagesReached);
        var stars = (int)Mathf.Lerp(0, 3, interStagesReached);
        Debug.Log(stars);
        var result = new AppriceResult(stars, 3, (int)dish.Score, 1, finalValue);
        _order.Apprice(result);
        Dismiss();
    }

    private void OnOrderSubmited()
    {
        OrdersHandler.instance.SubmitOrder(_order);
    }

    private void OnOrderCanceled()
    {
        Dismiss();
    }

    private void OnOrderDeclined()
    {
        Dismiss();
        OrdersHandler.instance.DeclineOrder(_order);
    }

    public override void Interract()
    {
    }

    public virtual void SetOrder(Order order)
    {
        _order = order;
        order.OnCompleted += OnOrderCompleted;
        order.OnCanceled += OnOrderCanceled;
        order.OnDeclined += OnOrderDeclined;
        order.OnSubmited += OnOrderSubmited;

        OrdersHandler.instance.StartOrderPreview(order);
        _mapObject.Hide();
    }
}

public static class TextMeshProUtills
{
    public static string Sprite(int index)
    {
        return "<sprite=" + index + ">";
    }
}