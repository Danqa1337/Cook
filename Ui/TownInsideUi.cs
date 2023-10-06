using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownInsideUi : UiCanvas
{
    private void OnEnable()
    {
        VoidClickCatcher.OnClick += OnVoidClick;
    }

    private void OnDisable()
    {
        VoidClickCatcher.OnClick -= OnVoidClick;
    }

    private void OnVoidClick()
    {
        GameStateManager.ChangeGameState(GameState.Cooking);
    }

    public void QuestButtonPressed()
    {
        if (DataHolder.CurrentData.CurrentSupplies.Count > 0 && OrdersHandler.instance.CurrentOrder == null)
        {
            GameStateManager.ChangeGameState(GameState.OrdersList);
        }
    }

    public void TravelButtonPressed()
    {
        GameStateManager.ChangeGameState(GameState.FastTravel);
    }

    public void TradeButtonPressed()
    {
        var person = PersonsHandler.instance.SpawnTrader();
        person.transform.position = Player.instance.transform.position;
    }
}