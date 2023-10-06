using System;
using System.Collections;
using UnityEngine;

public class SuppliesSpawner : Singleton<SuppliesSpawner>
{
    public static event Action<Supply> OnSpawned;

    public Supply Spawn(SupplyName supplyName)
    {
        var supplyObject = Instantiate(SuppliesDatabase.GetSupplyPrefab(supplyName).gameObject, transform.position, Quaternion.identity);
        var supply = supplyObject.gameObject.GetComponent<Supply>();
        OnSpawned?.Invoke(supply);
        return supply;
    }
}