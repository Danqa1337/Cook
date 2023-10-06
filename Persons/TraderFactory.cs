using System;
using System.Collections.Generic;
using System.Linq;

public enum PriceTier
{
    Normal,
    Discount,
    Overprice,
}

public static class TraderFactory
{
    public static SerializableDictionary<SupplyName, SupplyData> GetSupplies()
    {
        var supplies = new SerializableDictionary<SupplyName, SupplyData>();
        var allAwaibleSupplies = SuppliesDatabase.GetValidSupplyNames().Where(s => DataHolder.CurrentData.UnlockedSupplies.Contains(s));
        for (int i = 0; i < 3; i++)
        {
            var supplyName = allAwaibleSupplies.Where(s => !supplies.Contains(s)).ToArray().RandomItem();
            var baseSupplyData = SuppliesDatabase.GetSupplyData(supplyName);
            var priceTier = KatabasisUtillsClass.Chance(10) ? PriceTier.Discount : KatabasisUtillsClass.Chance(10) ? PriceTier.Overprice : PriceTier.Normal;
            var price = baseSupplyData.price;
            var quantity = (int)UnityEngine.Random.Range(1, 4);

            switch (priceTier)
            {
                case PriceTier.Normal:
                    price = (int)UnityEngine.Random.Range(price * 0.9f, price * 1.1f);
                    break;

                case PriceTier.Discount:
                    price = (int)UnityEngine.Random.Range(price * 0.4f, price * 0.6f);
                    break;

                case PriceTier.Overprice:
                    price = (int)UnityEngine.Random.Range(price * 1.8f, price * 2f);
                    break;
            }

            var newSupplyData = new SupplyData(1, quantity, price, null);

            supplies.Add(supplyName, newSupplyData);
        }
        return supplies;
    }
}