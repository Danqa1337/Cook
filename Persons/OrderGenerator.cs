using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class OrderGenerator
{
    public static Order GenerateOrder(float minOrderRad, float maxOrderRad)
    {
        var ingsAndSupplies = new Dictionary<SupplyData, StaticIngredientData>();
        Debug.Log(DataHolder.CurrentData.CurrentSupplies.Count);
        foreach (var keyValue in DataHolder.CurrentData.CurrentSupplies)
        {
            var ingredient = SuppliesDatabase.GetSupplyPrefab(keyValue.Key).Ingredient;
            if (ingredient != null)
            {
                var ingData = IngredientsDatabase.GetIngredientData(ingredient.IngredientName);
                if (ingData.movement != Vector2.zero)
                {
                    ingsAndSupplies.Add(keyValue.Value, ingData);
                }
            }
        }

        var targetDirection = Vector2.zero;
        var choosenSupplies = ingsAndSupplies.Keys.ToArray().RandomItems(3).Shuffle();

        for (int i = 0; i < 10; i++)
        {
            var supply = choosenSupplies.RandomItem();
            var ingData = ingsAndSupplies[supply];
            targetDirection += ingData.movement;
        }

        var playerPosition = Player.instance.transform.position.ToVector2();
        var targetPosition = playerPosition + targetDirection.normalized * UnityEngine.Random.Range(minOrderRad, maxOrderRad) * Player.instance.MovementScale;
        var targetDistance = targetPosition.magnitude;
        var interStages = new List<InterStageData>();
        var stagesCount = (targetDistance - 1) / 2f;
        for (int i = 0; i < stagesCount; i++)
        {
            var randomShift = (Vector3.Cross(Vector3.forward, targetDirection).normalized * UnityEngine.Random.Range(-targetDistance * 0.2f, targetDistance * 0.2f)).ToVector2();
            var stagePosition = Vector2.Lerp(playerPosition, targetPosition, (float)(i + 1f) / (float)(stagesCount + 1f)) + randomShift;
            var cell = Map.GetCell(stagePosition);

            interStages.Add(new InterStageData(cell));
        }

        return new Order(targetPosition, (int)targetDistance, interStages, new List<SupplyName>());
    }
}