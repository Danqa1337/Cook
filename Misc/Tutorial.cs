using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour, IGameStateChangeHandler
{
    [SerializeField] private Canvas _moveCanvas;
    [SerializeField] private Canvas _townCanvas;
    [SerializeField] private Canvas _orderPreviewCanvas;
    [SerializeField] private Canvas _ingredientsCanvas;
    [SerializeField] private Canvas _orderCanvas;
    private bool TutorialEnabled => true || DataHolder.CurrentData.CheatMode == false;

    private void OnEnable()
    {
        OrdersHandler.OnOrderPreviewStart += OnOrderPreviewStart;
        OrdersHandler.OnOrderDeclined += OnOrderPreviewEnd;
        OrdersHandler.OnOrderSubmited += OnOrderSubmited;
        OrdersHandler.OnOrderCompleted += OnOrderCompleted;
        SuppliesHandler.OnSupplyShown += OnSupplyShown;
        Kitchen.instance.ActiveCrockery.OnIngredientAdded += OnIngAdded;
    }

    private void OnDisable()
    {
        OrdersHandler.OnOrderPreviewStart -= OnOrderPreviewStart;
        OrdersHandler.OnOrderDeclined -= OnOrderPreviewEnd;
        OrdersHandler.OnOrderSubmited -= OnOrderSubmited;
        OrdersHandler.OnOrderCompleted -= OnOrderCompleted;
        SuppliesHandler.OnSupplyShown -= OnSupplyShown;
        Kitchen.instance.ActiveCrockery.OnIngredientAdded -= OnIngAdded;
    }

    private void Start()
    {
        var canvases = new Canvas[] { _moveCanvas, _townCanvas, _orderPreviewCanvas, _ingredientsCanvas, _orderCanvas };
        foreach (var item in canvases)
        {
            item.enabled = false;
        }
    }

    public void OnGameStateChanged(GameState gameState)
    {
        if (TutorialEnabled)
        {
            if (gameState == GameState.Cooking)
            {
                if (!DataHolder.CurrentData.TutorialData.moveTutorialComplete)
                {
                    _moveCanvas.enabled = true;
                }
            }
            if (gameState == GameState.Town)
            {
                if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.townTutorialComplete)
                {
                    _townCanvas.enabled = true;
                }
            }
        }
    }

    private void OnSupplyShown(SupplyName supplyName)
    {
        if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.moveTutorialComplete)
        {
            _moveCanvas.enabled = false;
            _ingredientsCanvas.enabled = true;
        }
    }

    private void OnIngAdded(Ingredient ingredient)
    {
        if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.moveTutorialComplete)
        {
            DataHolder.CurrentData.TutorialData.moveTutorialComplete = true;
            _ingredientsCanvas.enabled = false;
        }
    }

    private void OnOrderPreviewStart(Order order)
    {
        if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.townTutorialComplete)
        {
            DataHolder.CurrentData.TutorialData.townTutorialComplete = true;
            _townCanvas.enabled = false;
        }

        if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.orderTutorialComplete)
        {
            _orderPreviewCanvas.enabled = true;
        }
    }

    private void OnOrderPreviewEnd(Order order)
    {
        if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.orderTutorialComplete)
        {
            _orderPreviewCanvas.enabled = false;
            _orderCanvas.enabled = true;
        }
    }

    private void OnOrderSubmited(Order order)
    {
        if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.orderTutorialComplete)
        {
            _orderPreviewCanvas.enabled = false;
        }
    }

    private void OnOrderCompleted(Order order)
    {
        if (TutorialEnabled && !DataHolder.CurrentData.TutorialData.orderTutorialComplete)
        {
            DataHolder.CurrentData.TutorialData.orderTutorialComplete = true;
            _orderCanvas.enabled = false;
        }
    }

    public void Close()
    {
        foreach (var item in GetComponentsInChildren<Canvas>())
        {
            item.enabled = false;
        }
    }
}