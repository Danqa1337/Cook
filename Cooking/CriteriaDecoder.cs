public static class CriteriaDecoder
{
    public static Criteria GetCriteria(string criteriaString)
    {
        var ingredients = new SerializableDictionary<IngredientName, Amount>();
        var ingAmountPairs = criteriaString.Split(',');
        foreach (var pair in ingAmountPairs)
        {
            var splitpair = pair.Split("*");
            ingredients.Add(splitpair[0].DecodeCharSeparatedEnumsAndGetFirst<IngredientName>(), splitpair[1].DecodeCharSeparatedEnumsAndGetFirst<Amount>());
        }
        return new Criteria(ingredients);
    }
}