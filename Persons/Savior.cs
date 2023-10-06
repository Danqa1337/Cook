using System;
using System.Collections.Generic;

public class Savior : Trader
{
    public override PersonType PersonType => PersonType.Savior;

    public override event Action OnShowWares;

    public override string Name => "Внезапный спаситель";

    public override void OnSpawned()
    {
        _supplies = new SerializableDictionary<SupplyName, SupplyData>();
        var defaultData = DefaultDataFactory.GetDefaultData();
        _supplies = defaultData.CurrentSupplies;
    }

    public override void Interract()
    {
        var phrases = new List<string>();
        phrases.Add("Я вижу, ты остался без денег и без инредиентов. Чел, это позор.\r\nВот, возьми покушай.");
        phrases.Add("Чел, это позор.");
        phrases.Add("Вот, возьми покушай.");
        var answer = "Спасибо...";

        var dialog = new Dialog(this, phrases.ToArray(), answer, null);
        dialog.OnPositiveAnswer += OnPositive;

        DialogManager.StartDialog(dialog);
    }

    private void OnPositive()
    {
        OnShowWares?.Invoke();
    }

    public override void Dismiss()
    {
        base.Dismiss();
    }
}