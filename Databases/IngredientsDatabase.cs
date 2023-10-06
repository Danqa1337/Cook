using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "IngredientsDatabase", menuName = "Databases/IngredientsDatabase")]
public class IngredientsDatabase : Database<IngredientsDatabase, IngredientsTable, IngredientsTable.Param, StaticIngredientData>
{
    [SerializeField] private SerializableDictionary<IngredientName, StaticIngredientData> _ingredients = new SerializableDictionary<IngredientName, StaticIngredientData>();
    protected override string enumName => "IngredientName";

    public override void StartUp()
    {
    }

    public static Sprite GetIcon(IngredientName ingredientName)
    {
        var icon = SuppliesDatabase.GetIcon(ingredientName.ToString().DecodeCharSeparatedEnumsAndGetFirst<SupplyName>());
        return icon;
    }

    public static StaticIngredientData GetIngredientData(IngredientName ingredientName)
    {
        return instance._ingredients[ingredientName];
    }

    protected override void StartReimport()
    {
        base.StartReimport();
        _ingredients = new SerializableDictionary<IngredientName, StaticIngredientData>();
    }

    protected override void ProcessParam(IngredientsTable.Param param)
    {
        var data = new StaticIngredientData();
        var name = param.enumName.DecodeCharSeparatedEnumsAndGetFirst<IngredientName>();
        data.effect = param.Effect.DecodeCharSeparatedEnumsAndGetFirst<EffectName>();
        data.ingredientType = param.IngredientType.DecodeCharSeparatedEnumsAndGetFirst<IngredientType>();
        data.score = param.score;
        data.movement = new Vector2(param.movementX, param.movementY);
        data.power = param.power;

        _ingredients.Add(name, data);
    }
}