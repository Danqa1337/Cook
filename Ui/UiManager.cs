using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiManager : Singleton<UiManager>, IGameStateChangeHandler
{
    private UiCanvas[] _uiCanvases;

    public void Awake()
    {
        _uiCanvases = FindObjectsByType<UiCanvas>(FindObjectsSortMode.None);
    }

    public void ShowUI(UIName uIName)
    {
        Debug.Log("showing + " + uIName);
        HidAll();
        var canvas = GetCanvas(uIName);
        canvas.Show();
    }

    private void HidAll()
    {
        foreach (var item in _uiCanvases)
        {
            item.Hide();
        }
    }

    private UiCanvas GetCanvas(UIName uIName)
    {
        var canvas = _uiCanvases.FirstOrDefault(u => u.UiName == uIName);
        if (canvas != null)
        {
            return canvas;
        }
        else
        {
            throw new System.Exception("Canvas not found: " + uIName);
        }
    }

    public void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Cooking:
                ShowUI(UIName.Cooking);
                break;

            case GameState.Trading:
                ShowUI(UIName.Trade);
                break;

            case GameState.MainMenu:
                ShowUI(UIName.MainMenu);
                break;

            case GameState.SavesScreen:
                ShowUI(UIName.LoadGame);
                break;

            case GameState.Orders:
                ShowUI(UIName.OrderPreview);
                break;

            case GameState.Appriceing:
                ShowUI(UIName.Appricing);
                break;

            case GameState.FastTravel:
                ShowUI(UIName.FastTravel);
                break;

            case GameState.Town:
                ShowUI(UIName.Town);
                break;

            case GameState.OrdersList:
                ShowUI(UIName.OrdersList);
                break;

            default:
                break;
        }
    }
}