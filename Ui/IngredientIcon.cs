using UnityEngine;
using UnityEngine.UI;

public class IngredientIcon : MonoBehaviour
{
    [SerializeField] private Image[] _images;

    public void Draw(IngredientName ingredientName, Amount amount)
    {
        Clear();
        for (int i = 0; i < (int)amount + 1; i++)
        {
            var image = _images[i];
            image.sprite = IngredientsDatabase.GetIcon(ingredientName);
            image.color = Color.white;
        }
    }

    public void Clear()
    {
        foreach (var item in _images)
        {
            item.Clear();
        }
    }
}