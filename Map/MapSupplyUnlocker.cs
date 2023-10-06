using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MapObject))]
public class MapSupplyUnlocker : MapObjectComponent
{
    [SerializeField] private SupplyName _supplyName;
    [SerializeField][Min(1)] private int _count;

    protected override void OnPlayerEnter()
    {
        DataHolder.CurrentData.UnlockSupply(_supplyName);
        DataHolder.CurrentData.AddSupply(_supplyName, _count);
        Destroy(gameObject);
    }

    protected override void OnPlayerExit()
    {
    }

    protected override void OnPlayerStay()
    {
    }
}