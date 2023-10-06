using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dish
{
    public readonly int MaxCapacity;
    public float Score { get; private set; }
    public Stack<IngredientName> Ingredients { get; private set; }
    public bool IsFull => MaxCapacity <= Ingredients.Count;

    public Dish(int capacity)
    {
        Ingredients = new Stack<IngredientName>();
        MaxCapacity = capacity;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (Ingredients.Count == 0 || Ingredients.Peek() != ingredient.IngredientName && MaxCapacity > Ingredients.Count)
        {
            Ingredients.Push(ingredient.IngredientName);
        }
        Score += ingredient.StaticIngredientData.score * ingredient.Mass;
    }
}