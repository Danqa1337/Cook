using System;
using System.Collections.Generic;

public class Trader : Person
{
    protected SerializableDictionary<SupplyName, SupplyData> _supplies;
    public SerializableDictionary<SupplyName, SupplyData> Supplies { get => _supplies; }

    public override PersonType PersonType => PersonType.Trader;

    public virtual event Action OnShowWares;

    public virtual event Action OnWaresUpdated;

    public override string Name => "Торговец";

    public override void OnSpawned()
    {
        _supplies = TraderFactory.GetSupplies();
    }

    public override void Interract()
    {
        OnShowWares?.Invoke();
        //var phrases = new List<string>();
        //phrases.Add("Добрый день!");
        //phrases.Add("У меня есть для вас пециальное предложение!");

        //var dialog = new Dialog(this, phrases.ToArray(), "Ну-ка дай посмотреть", "Проваливай отсюда, пес");

        //dialog.OnPositiveAnswer += OnPositiveAnswer;
        //dialog.OnNegativeAnswer += OnNegativeAnswer;

        //DialogManager.StartDialog(dialog);
    }

    private void OnNegativeAnswer()
    {
        DataHolder.instance.Save();
        Dismiss();
    }

    private void OnPositiveAnswer()
    {
        OnShowWares?.Invoke();
    }

    public bool Buy(SupplyName supplyName)
    {
        var supplyData = _supplies[supplyName];
        var price = supplyData.price;
        if (DataHolder.CurrentData.Money >= price)
        {
            DataHolder.CurrentData.Money -= price;

            DataHolder.CurrentData.AddSupply(supplyName, 1);

            if (supplyData.quantity > 1)
            {
                _supplies[supplyName].quantity--;
            }
            else
            {
                _supplies.Remove(supplyName);
            }
            UnityEngine.Debug.Log("Bought " + supplyName);
            OnWaresUpdated?.Invoke();
            return true;
        }
        UnityEngine.Debug.Log("Not enough money to buy " + supplyName);
        return false;
    }
}