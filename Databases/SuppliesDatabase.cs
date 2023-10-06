using System;
using System.Linq;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "SuppliesDatabase", menuName = "Databases/SuppliesDatabase")]
public class SuppliesDatabase : Database<SuppliesDatabase, SuppliesTable, SuppliesTable.Param, SupplyData>
{
    [SerializeField] private SerializableDictionary<SupplyName, GameObject> _prefabsByNames;
    [SerializeField] private SerializableDictionary<SupplyName, SupplyData> _supplyDataByNames;
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private SpriteAtlas _iconsAtlas;
    protected override string enumName => "SupplyName";

    [SerializeField]
    public override void StartUp()
    {
    }

    public static SupplyName[] GetValidSupplyNames()
    {
        return instance._prefabsByNames.Values.Select(s => s.GetComponent<Supply>().SupplyName).ToArray();
    }

    public static Sprite GetIcon(SupplyName supplyName)
    {
        return instance._iconsAtlas.GetSprite(supplyName + "SupplyIcon");
    }

    public static Supply GetSupplyPrefab(SupplyName supplyName)
    {
        return instance._prefabsByNames[supplyName].GetComponent<Supply>();
    }

    public static SupplyData GetSupplyData(SupplyName supplyName)
    {
        return instance._supplyDataByNames[supplyName];
    }

    protected override void StartReimport()
    {
        base.StartReimport();
        _prefabsByNames = new SerializableDictionary<SupplyName, GameObject>();
        _supplyDataByNames = new SerializableDictionary<SupplyName, SupplyData>();
    }

    protected override void ProcessParam(SuppliesTable.Param param)
    {
        var name = param.enumName.DecodeCharSeparatedEnumsAndGetFirst<SupplyName>();
        var prefab = _prefabs.FirstOrDefault(p => p.GetComponent<Supply>().SupplyName == name);

        if (prefab != null)
        {
            var supplyData = new SupplyData(1, 1, param.Cost, null);
            _prefabsByNames.Add(name, prefab);
            _supplyDataByNames.Add(name, supplyData);
        }
    }
}