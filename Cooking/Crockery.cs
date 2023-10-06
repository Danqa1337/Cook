using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Amount
{
    Small,
    Medium,
    Large,
}

public abstract class Crockery : MonoBehaviour
{
    [SerializeField] protected Dish _dish;
    [SerializeField] private float _maxMass = 25;
    [SerializeField] private int _maxCapacity = 10;
    [SerializeField] private float _cookingSpeed;
    [SerializeField] private GameObject _cover;

    public event Action<Ingredient> OnIngredientAdded;

    public event Action<Ingredient> OnChunkTooBig;

    public event Action<Ingredient> OnIngredientRejected;

    public event Action<Dish> OnDishChanged;

    public event Action OnFull;

    public event Action<Combo> OnComboActivated;

    protected List<Ingredient> _ingredients = new List<Ingredient>();
    public float MaxMass { get => _maxMass; }
    public Dish Dish { get => _dish; }
    public List<Ingredient> Ingredients { get => _ingredients; }
    [SerializeField] private float _coverZPos;

    protected virtual void Start()
    {
        _coverZPos = _cover.transform.localPosition.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        var ing = other.gameObject.GetComponent<Ingredient>();
        if (ing != null)
        {
            AddIngredient(ing);
        }
    }

    public virtual void AddIngredient(Ingredient ingredient)
    {
        if (!_dish.IsFull)
        {
            _ingredients.Add(ingredient);
            Debug.Log("Added " + ingredient + " X " + ingredient.Mass);

            _dish.AddIngredient(ingredient);
            ingredient.OnAdded(this);
            OnIngredientAdded?.Invoke(ingredient);
            OnDishChanged?.Invoke(_dish);
        }
        else
        {
            OnFull?.Invoke();
            Debug.Log("Dish is full");
        }
        Destroy(ingredient.gameObject);
    }

    public virtual void RejectIngridient(Ingredient ingridient)
    {
        ingridient.OnRejected(this);
        OnIngredientRejected?.Invoke(ingridient);
    }

    public void Clear()
    {
        _dish = new Dish(_maxCapacity);
        OnDishChanged?.Invoke(_dish);
    }

    public void ChangeDish(Dish dish)
    {
        _dish = dish;
        OnDishChanged?.Invoke(dish);
    }

    public void Close()
    {
        _cover.transform.DOLocalMoveY(_coverZPos, 1);
    }

    public void Open()
    {
        _cover.transform.DOLocalMoveY(_coverZPos + 25, 1);
    }

    public Dish FinalizeDish()
    {
        Open();
        return Dish;
    }
}