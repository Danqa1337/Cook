using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UiCanvas
{
    private void Start()
    {
        GameStateManager.ChangeGameState(GameState.MainMenu);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        GameStateManager.ChangeGameState(GameState.SavesScreen);
    }
}