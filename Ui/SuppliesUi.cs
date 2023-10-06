using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SuppliesUi : MonoBehaviour
{
    [SerializeField] private SupplyIcon _supplyIconPrefab;
    [SerializeField] private Transform _layout;
    private List<SupplyIcon> _icons = new List<SupplyIcon>();

    private void OnEnable()
    {
        SuppliesHandler.OnOutOfSupply += OnEmpty;
        SuppliesHandler.OnSupplyQuantityChanged += OnQuantityChanged;
        SuppliesHandler.OnSupplyRemainingChanged += OnRemainingChanged;
        DataHolder.OnSupplyChanged += OnSuppliesChanged;
        DataHolder.OnLoaded += OnLoaded;
        OrdersHandler.OnOrderSubmited += OnOrderSubmited;
    }

    private void OnDisable()
    {
        SuppliesHandler.OnOutOfSupply -= OnEmpty;
        SuppliesHandler.OnSupplyQuantityChanged -= OnQuantityChanged;
        SuppliesHandler.OnSupplyRemainingChanged -= OnRemainingChanged;
        DataHolder.OnSupplyChanged -= OnSuppliesChanged;
        DataHolder.OnLoaded -= OnLoaded;
        OrdersHandler.OnOrderSubmited -= OnOrderSubmited;
    }

    private void OnOrderSubmited(Order order)
    {
        DrawSupplies();
    }

    private void OnLoaded(DataHolder.SaveData saveData)
    {
        DrawSupplies();
    }

    private void OnSuppliesChanged(SupplyName supplyName, SupplyData supplyData)
    {
        DrawSupplies();
    }

    private void DrawSupplies()
    {
        Clear();
        var supplies = DataHolder.CurrentData.CurrentSupplies.Keys;
        var lockedSupplies = new List<SupplyName>();
        var notLockedSupplies = new List<SupplyName>();

        foreach (var item in supplies)
        {
            var isLocked = OrdersHandler.instance.CurrentOrder != null && OrdersHandler.instance.CurrentOrder.ForbidenSupplies.Contains(item);
            if (isLocked) lockedSupplies.Add(item);
            else notLockedSupplies.Add(item);
        }

        foreach (var item in notLockedSupplies)
        {
            DrawSupply(item, false);
        }
        foreach (var item in lockedSupplies)
        {
            DrawSupply(item, true);
        }

        if (supplies.Count == 0)
        {
            Debug.Log("No supplies avaible");
        }
    }

    private void DrawSupply(SupplyName item, bool isLocked)
    {
        var newIcon = Instantiate(_supplyIconPrefab.gameObject).GetComponent<SupplyIcon>();
        newIcon.DrawSupply(item, DataHolder.CurrentData.CurrentSupplies[item], isLocked);
        newIcon.transform.SetParent(_layout.transform);
        newIcon.OnClick += delegate { OnClick(item); };
        newIcon.transform.localScale = Vector3.one;

        _icons.Add(newIcon);
    }

    private void Clear()
    {
        foreach (var item in _icons)
        {
            Destroy(item.gameObject);
        }
        _icons = new List<SupplyIcon>();
    }

    private void OnClick(SupplyName supplyName)
    {
        SuppliesHandler.instance.Show(supplyName);
    }

    private void OnQuantityChanged(SupplyName supplyName, int qua)
    {
        var icon = GetIcon(supplyName);
        icon.ChangeQuantity(qua);
    }

    private void OnRemainingChanged(SupplyName supplyName, float rem)
    {
        var icon = GetIcon(supplyName);
        icon.ChangeRemaining(rem);
    }

    private void OnEmpty(SupplyName supplyName)
    {
        var icon = GetIcon(supplyName);
        _icons.Remove(icon);
        icon.OnEmpty();
    }

    private SupplyIcon GetIcon(SupplyName supplyName)
    {
        var icon = _icons.FirstOrDefault(i => i.SupplyName == supplyName);
        return icon;
    }
}