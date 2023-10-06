using DG.Tweening;
using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class OrderMarker : MapObjectComponent
{
    public static event Action<float> OnPlayerStayDistance;

    [SerializeField] private OrderInterStage _orderInterStagePrefab;
    [SerializeField] private CameraFollowPoint _previewPoint;
    [SerializeField] private PathCreator _bezierPath;
    [SerializeField] private LineRenderer _lineRenderer;
    private Order _order;

    private void OnEnable()
    {
        OrdersHandler.OnOrderSubmited += OnOrderSubmited;
        OrdersHandler.OnOrderPreviewStart += OnStartPreviewOrder;
        OrdersHandler.OnOrderCanceled += OnOrderCanceled;
        OrdersHandler.OnOrderCompleted += OnOrderCompleted;
        OrdersHandler.OnOrderDeclined += OnOrderDeclined;
    }

    private void OnDisable()
    {
        OrdersHandler.OnOrderSubmited -= OnOrderSubmited;
        OrdersHandler.OnOrderPreviewStart -= OnStartPreviewOrder;
        OrdersHandler.OnOrderCanceled -= OnOrderCanceled;
        OrdersHandler.OnOrderCompleted -= OnOrderCompleted;
        OrdersHandler.OnOrderDeclined -= OnOrderDeclined;
    }

    private void Start()
    {
        _lineRenderer.positionCount = 0;
        _mapObject.Hide();
        _mapObject.SpriteRenderer.transform.DOShakeScale(1, 0.2f, 0, 90, false).SetLoops(-1);
    }

    protected override void OnPlayerEnter()
    {
        Kitchen.instance.FinalizeDish();
    }

    private void OnStartPreviewOrder(Order order)
    {
        _order = order;
        _mapObject.Show();
        transform.localPosition = order.TargetPosition;
        _previewPoint.Activate();
        var stages = new List<Vector2>() { Player.instance.transform.localPosition };

        foreach (var item in order.InterStages)
        {
            var interStage = Instantiate(_orderInterStagePrefab).GetComponent<OrderInterStage>();
            Map.instance.Spawn(interStage.gameObject, item.position);
            interStage.SetOrder(order);
            stages.Add(interStage.transform.localPosition.ToVector2());
        }
        stages.Add(order.TargetPosition);

        _bezierPath.bezierPath = new BezierPath(stages, false, PathSpace.xy);
        _bezierPath.TriggerPathUpdate();
        var linePoints = new List<Vector3>();
        for (int i = 0; i < _bezierPath.bezierPath.NumSegments; i++)
        {
            linePoints.AddRange(_bezierPath.bezierPath.GetPointsInSegment(i));
        }
        _lineRenderer.positionCount = linePoints.Count;
        _lineRenderer.SetPositions(linePoints.ToArray());
        _lineRenderer.transform.position = Player.instance.transform.position;
    }

    private void OnOrderCompleted(Order order)
    {
        _order = null;

        _lineRenderer.positionCount = 0;
        _mapObject.Hide();
    }

    private void OnOrderCanceled(Order order)
    {
        _order = null;

        _lineRenderer.positionCount = 0;
        _mapObject.Hide();
    }

    private void OnOrderDeclined(Order order)
    {
        _order = null;

        _lineRenderer.positionCount = 0;
        _mapObject.Hide();
    }

    private void OnOrderSubmited(Order order)
    {
        _order = order;

        _mapObject.Show();
        transform.localPosition = order.TargetPosition;
    }

    protected override void OnPlayerExit()
    {
    }

    protected override void OnPlayerStay()
    {
    }

    private void Update()
    {
        var distance = (transform.position - Player.instance.transform.position).sqrMagnitude;
        OnPlayerStayDistance?.Invoke(distance);
    }
}