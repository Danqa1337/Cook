using System;
using UnityEngine;

public class Shaker : Supply, IUsable
{
    [SerializeField] private Transform _spawnPoint;
    private float _remaining = 1;
    public override float Remaining => _remaining;

    public override StaticIngredientData IngData => _ingredient.StaticIngredientData;

    public override event Action<SupplyName> OnEmpty;

    public override event Action<SupplyName, float> OnRemainingChanged;

    public void OnUse()
    {
        _remaining = Mathf.Max(0, _remaining - 0.1f);
        var ingridient = Instantiate(_ingredient, _spawnPoint.position, Quaternion.identity);
        OnRemainingChanged?.Invoke(_supplyName, _remaining);
        if (_remaining == 0)
        {
            OnEmpty?.Invoke(_supplyName);
        }
    }

    public override void OnSpawned(SupplyData supplyData)
    {
    }
}