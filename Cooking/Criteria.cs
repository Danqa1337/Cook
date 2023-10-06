using System;

[Serializable]
public class Criteria
{
    public SerializableDictionary<IngredientName, Amount> Ingredients;

    public Criteria(SerializableDictionary<IngredientName, Amount> ingredients)
    {
        Ingredients = ingredients;
    }

    public bool Check(Dish dish)
    {
        //foreach (var ing in Ingredients.Keys)
        //{
        //    if (dish.Ingredients.ContainsKey(ing))
        //    {
        //        if ((int)dish.Ingredients[ing] < (int)Ingredients[ing])
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        return true;
    }
}