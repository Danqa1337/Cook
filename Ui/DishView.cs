using UnityEngine;
using DG.Tweening;

using System.Linq;

public class DishView : MonoBehaviour
{
    [SerializeField] private IngredientIcon _ingredientIconPrefab;
    [SerializeField] private Transform _layout;

    private void OnEnable()
    {
        Kitchen.instance.ActiveCrockery.OnDishChanged += DrawDish;
        Kitchen.instance.ActiveCrockery.OnFull += OnFull;
        Kitchen.OnCookingEnded += Clear;
    }

    private void OnDisable()
    {
        Kitchen.instance.ActiveCrockery.OnDishChanged -= DrawDish;
        Kitchen.instance.ActiveCrockery.OnFull -= OnFull;
        Kitchen.OnCookingEnded -= Clear;
    }

    private void OnFull()
    {
        _layout.transform.DOShakePosition(1f, 10);
    }

    private void DrawDish(Dish dish)
    {
        Clear();
        for (int i = 0; i < dish.Ingredients.Count; i++)
        {
            var icon = Instantiate(_ingredientIconPrefab.gameObject, _layout).GetComponent<IngredientIcon>();
            icon.transform.SetSiblingIndex(0);
            icon.Draw(dish.Ingredients.ToList()[i], Amount.Small);
        }
    }

    private void Clear()
    {
        foreach (var item in _layout.GetChildren())
        {
            Destroy(item.gameObject);
        }
    }
}