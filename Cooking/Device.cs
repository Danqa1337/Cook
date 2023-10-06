using System;
using UnityEngine;

public abstract class Device : Supply, IUsable
{
    [SerializeField] private StaticIngredientData _staticIngredientData;

    private float _remaining = 1;
    public override float Remaining => _remaining;

    public override event Action<SupplyName> OnEmpty;

    public override event Action<SupplyName, float> OnRemainingChanged;

    public override void OnSpawned(SupplyData supplyData)
    {
    }

    public abstract void OnUse();
}