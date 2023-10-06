using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePerson : Person
{
    public override PersonType PersonType => PersonType.Guide;

    public override string Name => "������ �����";

    public override void Interract()
    {
        DataHolder.instance.Save();

        var phrases = new List<string>();
        phrases.Add("����� ���������� � ��������� ����");
        phrases.Add("���� ������� ����� � ������, �������� ������ �����������, ����������� ������.");

        var dialog = new Dialog(this, phrases, "�������", "� ���������");

        dialog.OnPositiveAnswer += Dismiss;
        dialog.OnNegativeAnswer += OnNegativeAnswer;

        DialogManager.StartDialog(dialog);
    }

    public void OnNegativeAnswer()
    {
        UiManager.instance.ShowUI(UIName.MainMenu);
    }
}