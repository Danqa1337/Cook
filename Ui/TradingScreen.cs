using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using DG.Tweening;

public class TradingScreen : UiCanvas
{
    [SerializeField] private Transform _iconHolder;
    [SerializeField] private SupplyIcon _iconPrefab;
    [SerializeField] private Label _moneyLabel;
    private Trader _trader;
    private List<SupplyIcon> _icons = new List<SupplyIcon>();

    private void OnEnable()
    {
        PersonsHandler.OnTraderShowWares += Show;
    }

    private void OnDisable()
    {
        PersonsHandler.OnTraderShowWares -= Show;
    }

    public void Show(Trader trader)
    {
        GameStateManager.ChangeGameState(GameState.Trading);
        Clear();
        _trader = trader;
        _trader.OnWaresUpdated += delegate { UpdateWares(trader); };
        UpdateWares(trader);
    }

    private void UpdateWares(Trader trader)
    {
        _trader = trader;
        var traderSupplies = _trader.Supplies.Keys;
        var extraIcons = _icons.Where(i => !traderSupplies.Contains(i.SupplyName)).ToArray();
        for (int i = 0; i < extraIcons.Length; i++)
        {
            var icon = extraIcons[i];
            _icons.Remove(icon);
            icon.OnEmpty();
        }

        _moneyLabel.SetValue(DataHolder.CurrentData.Money);
        if (_trader.Supplies.Count > 0)
        {
            foreach (var supplyName in traderSupplies)
            {
                var supplyData = _trader.Supplies[supplyName];
                var icon = GetIcon(supplyName);
                icon.DrawSupply(supplyName, supplyData);
            }
        }
        else
        {
            transform.DOMoveX(transform.position.x, 0.5f).OnComplete(Dismiss);
        }
    }

    private SupplyIcon GetIcon(SupplyName supplyName)
    {
        var icon = _icons.FirstOrDefault(i => i.SupplyName == supplyName);
        if (icon == null)
        {
            icon = Instantiate(_iconPrefab, _iconHolder).GetComponent<SupplyIcon>();
            icon.OnClick += OnClick;

            void OnClick()
            {
                if (_trader.Buy(supplyName))
                {
                    icon.ActPositive();
                }
                else
                {
                    icon.ActNegative();
                }
            }
            _icons.Add(icon);
        }
        return icon;
    }

    private void Clear()
    {
        _icons.Clear();
        foreach (var item in _iconHolder.GetChildren())
        {
            Destroy(item);
        }
    }

    public void Dismiss()
    {
        Hide();
        GameStateManager.ChangeGameState(GameState.Cooking);
        _trader.Dismiss();
    }
}