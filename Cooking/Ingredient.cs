using UnityEngine;

public abstract class Ingredient : MonoBehaviour
{
    [SerializeField] protected IngredientName _ingredientName;
    protected Crockery _parentCrockery;
    public IngredientName IngredientName => _ingredientName;
    public StaticIngredientData StaticIngredientData => IngredientsDatabase.GetIngredientData(_ingredientName);
    public Crockery ParentCrockery => _parentCrockery;
    public abstract float Mass { get; }

    public virtual void OnAdded(Crockery crockery)
    {
        _parentCrockery = crockery;
    }

    public virtual void OnRejected(Crockery crockery)
    {
        _parentCrockery = null;
    }

    public virtual void OnHeating(float value)
    {
    }

    public virtual void OnNegative()
    {
    }
}