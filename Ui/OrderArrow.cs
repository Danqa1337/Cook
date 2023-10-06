using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OrderArrow : MonoBehaviour
{
    [SerializeField] private OrderMarker _marker;
    [SerializeField] private Image _arrow;
    [SerializeField] private RectTransform _arrowCanvasRect;
    [SerializeField] private int _borderOffset = 50;
    private bool _active;

    private void OnEnable()
    {
        OrdersHandler.OnOrderCanceled += OnOrderEnded;
        OrdersHandler.OnOrderCompleted += OnOrderEnded;
        OrdersHandler.OnOrderSubmited += OnOrderSubmited;
    }

    private void OnDisable()
    {
        OrdersHandler.OnOrderCanceled -= OnOrderEnded;
        OrdersHandler.OnOrderCompleted -= OnOrderEnded;
        OrdersHandler.OnOrderSubmited -= OnOrderSubmited;
    }

    private void Start()
    {
        _arrow.enabled = false;
        _arrow.transform.DOShakeScale(1, 0.2f, 0, 90, false).SetLoops(-1);
    }

    private void Update()
    {
        if (_active && ShouldShowMarker())
        {
            _arrow.enabled = true;

            _arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, _marker.transform.position - Player.instance.transform.position);
            var _pointerRectTransform = _arrow.GetComponent<RectTransform>();
            var screenPoint = MainCameraController.Camera.WorldToScreenPoint(_marker.transform.position);
            var area = GetClampedArea();

            var capedScreenPoint = new Vector2(Mathf.Clamp(screenPoint.x, area.x, area.y), Mathf.Clamp(screenPoint.y, area.z, area.w));

            _pointerRectTransform.position = capedScreenPoint;
        }
        else
        {
            _arrow.enabled = false;
        }
    }

    private Vector4 GetClampedArea()
    {
        var x = _arrowCanvasRect.anchoredPosition.x * 2 + _borderOffset;
        var y = _arrowCanvasRect.anchoredPosition.x * 2 + _arrowCanvasRect.sizeDelta.x - _borderOffset;
        var z = _arrowCanvasRect.anchoredPosition.y * 2 + _borderOffset;
        var w = _arrowCanvasRect.anchoredPosition.y * 2 + _arrowCanvasRect.sizeDelta.y - _borderOffset;
        return new Vector4(x, y, z, w);
    }

    private void OnOrderSubmited(Order order)
    {
        _arrow.enabled = true;
        _active = true;
    }

    private void OnOrderEnded(Order order)
    {
        _arrow.enabled = false;
        _active = false;
    }

    private bool ShouldShowMarker()
    {
        var viewPos = MainCameraController.Camera.WorldToScreenPoint(_marker.transform.position);
        var area = GetClampedArea();
        if (viewPos.x >= area.x && viewPos.x <= area.y && viewPos.y >= area.z && viewPos.y <= area.w)
        {
            return false;
        }
        return true;
    }
}