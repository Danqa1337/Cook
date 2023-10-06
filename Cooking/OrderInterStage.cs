using DG.Tweening;
using System;
using UnityEngine;

public class OrderInterStage : MapObjectComponent
{
    public event Action OnReached;

    private Order _order;

    public void SetOrder(Order order)
    {
        _order = order;
        _order.OnCanceled += OnOrderEnded;
        _order.OnDeclined += OnOrderEnded;
        _order.OnCompleted += OnOrderCompleted;
    }

    private void OnDestroy()
    {
        if (_order != null)
        {
            _order.OnCanceled += OnOrderEnded;
            _order.OnDeclined += OnOrderEnded;
            _order.OnCompleted += OnOrderCompleted;
        }
    }

    private void Start()
    {
        _mapObject.SpriteRenderer.transform.DOShakeScale(1, 0.2f, 0, 90, false).SetLoops(-1);
    }

    private void OnOrderEnded()
    {
        Destroy(gameObject);
    }

    private void OnOrderCompleted(Dish dish)
    {
        OnOrderEnded();
    }

    protected override void OnPlayerEnter()
    {
        OnReached?.Invoke();
        if (_order != null)
        {
            _order.CompleteInterStage(transform.localPosition);
        }
        var seq = DOTween.Sequence();
        seq.Append(_mapObject.SpriteRenderer.transform.DOPunchScale(Vector3.one * 3, 0.2f));
        seq.AppendCallback(delegate { _mapObject.Hide(); });
        seq.Play();
    }

    protected override void OnPlayerExit()
    {
    }

    protected override void OnPlayerStay()
    {
    }
}