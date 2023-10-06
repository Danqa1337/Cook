using UnityEngine;

public class OrderListUi : UiCanvas
{
    [SerializeField] private Transform _layout;
    [SerializeField] private OrderIcon _orderIconPrefab;

    public override void Show()
    {
        base.Show();
        Clear();
        var town = Town.Current;
        foreach (var item in town.PresetOrders)
        {
            var icon = Instantiate(_orderIconPrefab.gameObject, _layout.transform).GetComponent<OrderIcon>();
            icon.Draw(item);
            icon.OnClicked += OnIconClicked;
        }
    }

    public void Clear()
    {
        foreach (var item in _layout.GetChildren())
        {
            Destroy(item.gameObject);
        }
    }

    public void Back()
    {
        GameStateManager.Back();
    }

    private void OnIconClicked(PresetOrder presetOrder)
    {
        var person = PersonsHandler.instance.SpawnCustomer();
        var orderCopy = new Order(presetOrder.Order.TargetPosition, presetOrder.Order.BaseReward, presetOrder.Order.InterStages, presetOrder.Order.ForbidenSupplies);
        var orderData = DataHolder.CurrentData.PresetOrders[presetOrder.name];
        orderCopy.OnCompleted += OnCompleted;
        orderCopy.OnAppriced += OnAppriced;

        person.SetOrder(orderCopy);
        person.transform.position = Player.instance.transform.position;

        void OnAppriced(AppriceResult appriceResult)
        {
            orderData.AppriceResult = appriceResult;
        }

        void OnCompleted(Dish dish)
        {
            orderData.Completed = true;
        }
    }
}