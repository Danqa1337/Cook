using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientsListUI : MonoBehaviour
{
    [SerializeField] private Transform _ingredientsIconsHolder;
    [SerializeField] private IngredientIcon _ingredientIconPrefab;

    public void DrawIngredients(IDictionary<IngredientName, Amount> ingredients)
    {
        Clear();
        foreach (var item in ingredients.Keys.OrderByDescending(i => (int)i))
        {
            var newIcon = Instantiate(_ingredientIconPrefab.gameObject, _ingredientsIconsHolder).GetComponent<IngredientIcon>();
            newIcon.Draw(item, ingredients[item]);
        }
    }

    public void Clear()
    {
        foreach (var item in _ingredientsIconsHolder.GetChildren())
        {
            Destroy(item.gameObject);
        }
    }
}