using System;
using UnityEngine;

public abstract class Supply : MonoBehaviour
{
    [SerializeField] protected SupplyName _supplyName;
    [SerializeField] protected Ingredient _ingredient;
    public abstract StaticIngredientData IngData { get; }
    public virtual Ingredient Ingredient { get => _ingredient; }

    public SupplyName SupplyName { get => _supplyName; }
    public bool IsEmpty => Remaining == 0;
    public bool IsFull => Remaining == 1;

    public abstract event Action<SupplyName> OnEmpty;

    public abstract event Action<SupplyName, float> OnRemainingChanged;

    public abstract float Remaining { get; }

    public abstract void OnSpawned(SupplyData supplyData);

    public virtual SupplyData GetSupplyData()
    {
        var data = new SupplyData();
        data.remaining = Remaining;
        data.quantity = DataHolder.CurrentData.CurrentSupplies[SupplyName].quantity;
        return data;
    }
}

public interface IUsable
{
    public void OnUse();
}