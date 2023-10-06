using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SupplyIcon : Selectable
{
    [SerializeField] private Image _icon;
    [SerializeField] private Label _priceLabel;
    [SerializeField] private Label _quantityLabel;
    [SerializeField] private Image _remainingFiller;
    [SerializeField] private Gradient _remainingGradient;
    [SerializeField] private Image _lockIcon;
    private bool _active = true;
    private SupplyName _supplyName;
    private int _quantity;
    private float _remaining;
    private int _price;
    private bool _isLocked;
    public SupplyName SupplyName { get => _supplyName; }

    public event Action OnClick;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_active && _quantity > 0)
        {
            if (!_isLocked)
            {
                base.OnPointerDown(eventData);
                OnClick?.Invoke();
                ActPositive();
            }
            else
            {
                ActNegative();
            }
        }
    }

    public virtual void DrawSupply(SupplyName supplyName, SupplyData supplyData, bool locked = false)
    {
        _isLocked = locked;
        _supplyName = supplyName;
        _icon.sprite = SuppliesDatabase.GetIcon(supplyName);

        ChangeRemaining(supplyData.remaining);
        ChangeQuantity(supplyData.quantity);
        ChangePrice(supplyData.price);
        if (locked)
        {
            _lockIcon.enabled = true;
            interactable = false;
        }
        else
        {
            _lockIcon.enabled = false;
            interactable = true;
        }
    }

    public void ChangePrice(int price)
    {
        _price = price;
        _priceLabel.SetValue(price);
        _priceLabel.SetColor(_price <= DataHolder.CurrentData.Money ? _priceLabel.DefaultColor : Color.red);
    }

    public void ChangeQuantity(int qua)
    {
        _quantity = qua;
        _quantityLabel.SetValue(qua);
    }

    public void ChangeRemaining(float rem)
    {
        _remaining = rem;
        _remainingFiller.fillAmount = rem;
        _remainingFiller.color = _remainingGradient.Evaluate(rem);
    }

    public void OnEmpty()
    {
        _active = false;
        Disapear();
    }

    public void ActNegative()
    {
        _icon.transform.DOKill();
        _icon.transform.localScale = Vector3.one;
        _icon.transform.DOShakePosition(0.5f, 2);
    }

    public void ActPositive()
    {
        _icon.transform.DOKill();
        _icon.transform.localScale = Vector3.one;
        _icon.transform.DOShakeScale(0.5f, 1);
    }

    protected override void OnDestroy()
    {
        transform.DOKill();
    }

    public void Disapear()
    {
        transform.DOScale(Vector3.zero, 0.3f).OnComplete(delegate { Destroy(gameObject); });
    }
}