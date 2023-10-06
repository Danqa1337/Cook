using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComboIcon : Selectable
{
    [SerializeField] private Image _image;
    private ComboName _comboName;

    public event Action<ComboName> OnClick;

    private RectTransform _rectTransform => transform as RectTransform;

    public void DrawCombo(ComboName comboName, bool opened)
    {
        _comboName = comboName;
        _image.sprite = CombosDatabase.GetIcon(comboName);
        if (!opened)
        {
            _image.color = Color.black;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnClick?.Invoke(_comboName);
    }
}