using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SupplyIconWithPrice : Selectable
{
    [SerializeField] private Image _image;
    [SerializeField] private Label _label;

    public static event Action<IngredientName, int> OnClick;

    private int _price;
    private IngredientName _ingredientName;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnClick.Invoke(_ingredientName, _price);
    }

    public void Draw(IngredientName ingredientName, int price)
    {
        _ingredientName = ingredientName;
        _price = price;
        _image.sprite = IngredientsDatabase.GetIcon(ingredientName);
        _label.SetText(price.ToString());
    }
}