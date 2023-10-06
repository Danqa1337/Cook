using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UIName
{
    Cooking,
    OrderPreview,
    Combos,
    Trade,
    Appricing,
    LoadGame,
    MainMenu,
    Dialog,
    Heating,
    FastTravel,
    Town,
    OrdersList,
}

public enum GameState
{
    Cooking,
    Trading,
    Orders,
    MainMenu,
    SavesScreen,
    Appriceing,
    FastTravel,
    Town,
    OrdersList,
}

public class GameStateManager : Singleton<GameStateManager>
{
    public static event Action<GameState> OnGameStateChanged;

    private Stack<GameState> _history = new Stack<GameState>();
    public static GameState CurrentGameState { get => instance._currentGameState; private set => instance._currentGameState = value; }
    private IGameStateChangeHandler[] _listeners;
    [SerializeField] private GameState _currentGameState;

    private void Awake()
    {
        _listeners = FindObjectsOfType<MonoBehaviour>().Where(t => t is IGameStateChangeHandler).Select(t => t as IGameStateChangeHandler).ToArray();
    }

    public static void ChangeGameState(GameState gameState)
    {
        instance._history.Push(CurrentGameState);
        SetGameState(gameState);
    }

    public static void Back()
    {
        SetGameState(instance._history.Pop());
    }

    private static void SetGameState(GameState gameState)
    {
        CurrentGameState = gameState;
        OnGameStateChanged?.Invoke(gameState);
        foreach (var item in instance._listeners)
        {
            item.OnGameStateChanged(gameState);
        }
    }
}

public interface IGameStateChangeHandler
{
    public abstract void OnGameStateChanged(GameState gameState);
}