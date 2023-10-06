using System.Collections.Generic;
using UnityEngine;

public class OrdersPreviewer : MonoBehaviour
{
    [SerializeField] private CameraFollowPoint _previewPoint;
    [SerializeField] private SupplyIcon _supplyIconPrefab;
    [SerializeField] private Transform _forbiddenIconsLayout;
    private Order _order;

    private void OnEnable()
    {
        OrdersHandler.OnOrderPreviewStart += StartOrderPreview;
    }

    private void OnDisable()
    {
        OrdersHandler.OnOrderPreviewStart -= StartOrderPreview;
    }

    private void StartOrderPreview(Order order)
    {
        Clear();
        GameStateManager.ChangeGameState(GameState.Orders);
        _order = order;
        foreach (var item in order.ForbidenSupplies)
        {
            var newIcon = Instantiate(_supplyIconPrefab.gameObject, _forbiddenIconsLayout).GetComponent<SupplyIcon>();
            newIcon.DrawSupply(item, DataHolder.CurrentData.CurrentSupplies[item], true);
            newIcon.transform.localScale = Vector3.one;
        }
    }

    private void Clear()
    {
        foreach (var item in _forbiddenIconsLayout.GetChildren())
        {
            Destroy(item.gameObject);
        }
    }

    public void SubmitOrder()
    {
        _order.Submit();
    }

    public void DeclineOrder()
    {
        _order.Decline();
        GameStateManager.Back();
    }
}