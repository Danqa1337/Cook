using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TownOutsideUi : MonoBehaviour, IGameStateChangeHandler
{
    [SerializeField] private Town _town;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _openButton;
    [SerializeField] private CameraFollowPoint _cameraFollowPoint;
    private bool _stay;

    private void OnEnable()
    {
        _town.OnPlayerEnterEvent += OnPlayerEnter;
        _town.OnPlayerExitEvent += OnPlayerExit;
    }

    private void OnDisable()
    {
        _town.OnPlayerEnterEvent -= OnPlayerEnter;
        _town.OnPlayerExitEvent -= OnPlayerExit;
    }

    private void Start()
    {
        _openButton.transform.DOShakeScale(1, 0.2f, 0, 90, false).SetLoops(-1);
        _canvas.enabled = false;
    }

    public void OpenTown()
    {
        _canvas.enabled = false;
        _cameraFollowPoint.Activate();
        GameStateManager.ChangeGameState(GameState.Town);
    }

    private void OnPlayerEnter()
    {
        _stay = true;
        _canvas.enabled = true;
    }

    private void OnPlayerExit()
    {
        _stay = false;
        _canvas.enabled = false;
    }

    public void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Cooking && _stay)
        {
            _canvas.enabled = true;
        }
    }
}