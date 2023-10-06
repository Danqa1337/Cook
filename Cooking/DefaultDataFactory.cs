using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public static class DefaultDataFactory
{
    public static DataHolder.SaveData GetDefaultData()
    {
        var saveData = GetBaseData();
        var currentSupplies = new SerializableDictionary<SupplyName, SupplyData>
        {
            {SupplyName.Sausage, new SupplyData{quantity = 1, remaining = 1 } },
           // {SupplyName.GreenFruit, new SupplyData{quantity = 5,  remaining = 1 } },
            //{SupplyName.SaltShaker, new SupplyData{quantity = 1,  remaining = 1 } },
            //{SupplyName.Peper, new SupplyData{quantity = 1,  remaining = 1 } },
        };
        foreach (var item in currentSupplies.Keys)
        {
            saveData.SetSupply(item, currentSupplies[item]);
        }
        var unlockedSupplies = new List<SupplyName>
        {
            SupplyName.Sausage,
            SupplyName.GreenFruit,
            SupplyName.Salt,
            SupplyName.Pepper,
        };

        foreach (var item in unlockedSupplies)
        {
            saveData.UnlockSupply(item);
        }

        saveData.Money = 100;
        return saveData;
    }

    private static DataHolder.SaveData GetBaseData()
    {
        var sites = Map.instance.Sites;

        var sitesData = new SerializableDictionary<float2, MapSiteData>();

        for (int i = 0; i < sites.Length; i++)
        {
            sitesData.Add(sites[i].transform.localPosition.ToFloat2(), new MapSiteData(sites[i].MapSiteType));
        }
        var saveData = new DataHolder.SaveData(sitesData);
        saveData.SetPlayerPosition(Map.WorldToLocal(Map.BaseSpawnPoint.position));
        return saveData;
    }

    public static DataHolder.SaveData GetAllSuppliesData()
    {
        var saveData = GetBaseData();
        foreach (var item in SuppliesDatabase.GetValidSupplyNames())
        {
            saveData.SetSupply(item, new SupplyData() { quantity = 99, remaining = 1 });
            saveData.UnlockSupply(item);
        }
        saveData.Money = 999;
        saveData.CheatMode = true;
        saveData.TutorialData.moveTutorialComplete = true;
        saveData.TutorialData.townTutorialComplete = true;
        saveData.TutorialData.orderTutorialComplete = true;
        return saveData;
    }
}