using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePerson : Person
{
    public override PersonType PersonType => PersonType.Guide;

    public override string Name => "Старый повар";

    public override void Interract()
    {
        DataHolder.instance.Save();

        var phrases = new List<string>();
        phrases.Add("Добро пожаловать в симулятор супа");
        phrases.Add("Режь колбасу мелко и быстро, выполняй заказы посетителей, зарабатывай деньги.");

        var dialog = new Dialog(this, phrases, "Поехали", "Я передумал");

        dialog.OnPositiveAnswer += Dismiss;
        dialog.OnNegativeAnswer += OnNegativeAnswer;

        DialogManager.StartDialog(dialog);
    }

    public void OnNegativeAnswer()
    {
        UiManager.instance.ShowUI(UIName.MainMenu);
    }
}