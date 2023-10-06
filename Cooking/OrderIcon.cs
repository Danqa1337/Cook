using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private PresetOrder _presetOrder;
    [SerializeField] private AppriceResultIcon _appriceResultIcon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Color _completedColor;
    [SerializeField] private Color _unCompletedColor;

    public event Action<PresetOrder> OnClicked;

    public void Draw(PresetOrder presetOrder)
    {
        _nameText.text = presetOrder.name;
        _presetOrder = presetOrder;
        _appriceResultIcon.Draw(DataHolder.CurrentData.PresetOrders[presetOrder.name].AppriceResult);
    }

    public void Click()
    {
        OnClicked?.Invoke(_presetOrder);
    }
}