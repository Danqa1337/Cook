using System;
using System.Linq;
using UnityEngine;

public class Kitchen : Singleton<Kitchen>, IGameStateChangeHandler
{
    [SerializeField] private Crockery _activeCrockery;
    [SerializeField] private float _minChunkMass = 0.1f;
    [SerializeField] private float _startDishMass = 5f;
    [SerializeField] private float _maxChunkMass = 1;
    [SerializeField] private float _maxCookingTime = 120;
    [SerializeField] private bool _creativeMode;
    [SerializeField] private Combo[] _validCombos;

    private float _markerDistance;

    public static event Action OnAllIngredientsAdded;

    public static event Action OnCookingEnded;

    public static event Action<Dish> OnDishFinalized;

    public Crockery ActiveCrockery { get => _activeCrockery; }
    public float MinChunkMass { get => _minChunkMass; }
    public float MaxChunkMass { get => _maxChunkMass; }
    public Combo[] ValidCombos { get => _validCombos; }
    public float StartDishMass { get => _startDishMass; }

    private void OnEnable()
    {
        Player.OnDied += StartNewDish;
    }

    private void OnDisable()
    {
        Player.OnDied -= StartNewDish;
    }

    public static SupplyName[] GetAwaibleSupplies()
    {
        return SuppliesDatabase.GetValidSupplyNames();
    }

    public void FinalizeDish()
    {
        OnCookingEnded?.Invoke();
        var dish = _activeCrockery.FinalizeDish();

        DataHolder.CurrentData.DishesCooked++;
        DataHolder.instance.Save();

        OnDishFinalized?.Invoke(dish);
    }

    public void StartNewDish()
    {
        Debug.Log("New dish started");
        _validCombos = CombosDatabase.GetAllCombos();
        _activeCrockery.Clear();
        _activeCrockery.Open();
        GameStateManager.ChangeGameState(GameState.Cooking);
    }

    public void Quit()
    {
        DataHolder.instance.Save();
        GameStateManager.ChangeGameState(GameState.MainMenu);
    }

    public void OnGameStateChanged(GameState gameState)
    {
        _activeCrockery.gameObject.SetActive(gameState == GameState.Cooking);
    }
}