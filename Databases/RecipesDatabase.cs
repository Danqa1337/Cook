using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipesDatabase", menuName = "Databases/RecipesDatabase")]
public class RecipesDatabase : Database<RecipesDatabase, RecipesTable, RecipesTable.Param, Order>
{
    protected override string enumName => "RecipeName";

    public override void StartUp()
    {
    }

    protected override void ProcessParam(RecipesTable.Param param)
    {
    }
}