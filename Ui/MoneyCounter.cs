using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void OnEnable()
    {
        DataHolder.OnMoneyChanged += DrawMoney;
        DataHolder.OnLoaded += OnLoaded;
    }

    private void OnDisable()
    {
        DataHolder.OnMoneyChanged -= DrawMoney;
        DataHolder.OnLoaded -= OnLoaded;
    }

    private void OnLoaded(DataHolder.SaveData saveData)
    {
        DrawMoney(saveData.Money);
    }

    private void DrawMoney(int money)
    {
        _moneyText.text = money.ToString() + "$";
    }
}