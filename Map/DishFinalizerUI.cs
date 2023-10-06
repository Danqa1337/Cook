using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DishFinalizerUI : MonoBehaviour, IGameStateChangeHandler
{
    [SerializeField] private TextMeshProUGUI _distanceText;
    [SerializeField] private Button _finalizeButton;
    [SerializeField] private Button _backToTownButton;

    private void OnEnable()
    {
        OrderMarker.OnPlayerStayDistance += ListenPlayerDistance;
        OrdersHandler.OnOrderSubmited += OnOrderSubmited;
        OrdersHandler.OnOrderCompleted += OnOrderEnded;
        OrdersHandler.OnOrderCanceled += OnOrderEnded;
    }

    private void OnDisable()
    {
        OrderMarker.OnPlayerStayDistance -= ListenPlayerDistance;
        OrdersHandler.OnOrderSubmited -= OnOrderSubmited;
        OrdersHandler.OnOrderCompleted -= OnOrderEnded;
        OrdersHandler.OnOrderCanceled -= OnOrderEnded;
    }

    private void Start()
    {
        _finalizeButton.gameObject.SetActive(false);
    }

    private void OnOrderSubmited(Order order)
    {
        _backToTownButton.gameObject.SetActive(false);
        _finalizeButton.gameObject.SetActive(true);
    }

    private void OnOrderEnded(Order order)
    {
        _backToTownButton.gameObject.SetActive(true);
        _finalizeButton.gameObject.SetActive(false);
    }

    private void ListenPlayerDistance(float distance)
    {
    }

    public void FinalizeDish()
    {
        Kitchen.instance.FinalizeDish();
    }

    public void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Cooking)
        {
            if (DataHolder.CurrentData.TutorialData.moveTutorialComplete)
            {
                _backToTownButton.gameObject.SetActive(true);
            }
            else
            {
                _backToTownButton.gameObject.SetActive(false);
            }
        }
    }
}