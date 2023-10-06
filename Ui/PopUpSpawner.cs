using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSpawner : Singleton<PopUpSpawner>
{
    [SerializeField] private AddSupplyPopUp _addSupplyPopUpPrefab;
    [SerializeField] private UnlockSupplyPopup _unlockSupplyPopUpPrefab;
    [SerializeField] private SliceComboPopup _sliceComboPopup;

    [SerializeField] private Transform _holder;
    private Queue<MovingPopup> _popupsQueue = new Queue<MovingPopup>();
    private bool _ready = true;

    private void OnEnable()
    {
        DataHolder.OnSupplyAdded += OnSupplyAdded;
        DataHolder.OnSupplyUnlocked += OnSupplyUnlocked;
        SliceComboManager.OnComboStarted += OnComboStarted;
    }

    private void OnDisable()
    {
        DataHolder.OnSupplyAdded -= OnSupplyAdded;
        DataHolder.OnSupplyUnlocked -= OnSupplyUnlocked;
        SliceComboManager.OnComboStarted -= OnComboStarted;
    }

    private void OnSupplyAdded(SupplyName supplyName, int count)
    {
        if (GameStateManager.CurrentGameState == GameState.Cooking)
        {
            var popup = SpawnAddSupplyPopUp();
            popup.Draw(supplyName, count);
        }
    }

    private void OnSupplyUnlocked(SupplyName supplyName)
    {
        var popup = SpawnUnlockSupplyPopUp();
        popup.Draw(supplyName);
    }

    private void OnComboStarted(int value, Vector2 position)
    {
        var go = Instantiate(_sliceComboPopup.gameObject, _holder);
        var movingPopup = go.GetComponent<MovingPopup>();
        var comboPopup = go.GetComponent<SliceComboPopup>();
        comboPopup.Draw(value, position);
    }

    public AddSupplyPopUp SpawnAddSupplyPopUp()
    {
        var go = Instantiate(_addSupplyPopUpPrefab.gameObject, _holder);
        var popup = go.GetComponent<MovingPopup>();
        popup.Canvas.enabled = false;
        _popupsQueue.Enqueue(popup);

        return go.GetComponent<AddSupplyPopUp>();
    }

    public UnlockSupplyPopup SpawnUnlockSupplyPopUp()
    {
        var go = Instantiate(_unlockSupplyPopUpPrefab.gameObject, _holder);
        var popup = go.GetComponent<MovingPopup>();
        popup.Canvas.enabled = false;
        _popupsQueue.Enqueue(popup);
        return go.GetComponent<UnlockSupplyPopup>();
    }

    private void Update()
    {
        if (_popupsQueue.Count > 0 && _ready)
        {
            _ready = false;
            var popup = _popupsQueue.Dequeue();
            popup.Canvas.enabled = true;
            var sequence = popup.Launch();
            sequence.AppendCallback(delegate { _ready = true; });
            sequence.Play();
        }
    }
}

public abstract class MovingPopup : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    public RectTransform RectTransform => transform as RectTransform;
    public Canvas Canvas { get => _canvas; set => _canvas = value; }

    public virtual Sequence Launch()
    {
        Debug.Log("Popup launched");
        var transform = GetComponent<RectTransform>();
        transform.anchoredPosition = new Vector2(0, Screen.height * -0.5f);
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOAnchorPosY(0, 0.5f));
        sequence.AppendInterval(1f);
        sequence.Append(transform.DOAnchorPosY(Screen.height * 0.5f, 0.5f));
        sequence.AppendCallback(delegate { Destroy(gameObject); });
        return sequence;
    }
}