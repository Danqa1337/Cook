using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchController : Singleton<TouchController>
{
    [SerializeField] private bool _debug;

    public static event Action<Vector2> OnTouchStart;

    public static event Action<Vector2> OnTouchEnd;

    public static event Action<Vector2> OnTouchPositionChanged;

    public static event Action<Vector2> OnManualInput;

    public static event Action<float> OnZoom;

    public static event Action<Vector2> OnDeltaChanged;

    private Controlls _controlls;
    private Vector2 _pointerPos;
    private bool _isTouchDown;
    private bool _zooming;
    private float _lastZoomDst;
    private bool _primaryTouchActive;
    private bool _secondaryTouchActive;

    private void Awake()
    {
        _controlls = new Controlls();
        _controlls.Touch.PrimaryTouchContact.started += _ => _primaryTouchActive = true;
        _controlls.Touch.PrimaryTouchContact.canceled += _ => _primaryTouchActive = false;
        _controlls.Touch.SecondaryTouchContact.started += _ => _secondaryTouchActive = true;
        _controlls.Touch.SecondaryTouchContact.canceled += _ => _secondaryTouchActive = false;
    }

    private void OnEnable()
    {
        _controlls.Enable();
    }

    private void OnDisable()
    {
        _controlls.Disable();
    }

    private void FixedUpdate()
    {
        ReadManualInput();
        ReadTouchInput();
    }

    private void ReadTouchInput()
    {
        var primaryTouchPhase = _controlls.Touch.PrimaryTouchPhase.ReadValue<UnityEngine.InputSystem.TouchPhase>();
        var secondTouchPhase = _controlls.Touch.SecondaryTouchPhase.ReadValue<UnityEngine.InputSystem.TouchPhase>();
        var primaryTouchPosition = _controlls.Touch.PrimaryTouchPosition.ReadValue<Vector2>();

        if (_primaryTouchActive && _secondaryTouchActive)
        {
            var secondaryTouchPosition = _controlls.Touch.SecondaryTouchPosition.ReadValue<Vector2>();
            var currentZommDst = Vector2.Distance(secondaryTouchPosition, primaryTouchPosition);
            if (_zooming)
            {
                var value = (currentZommDst - _lastZoomDst) * -1;
                Debug.Log(value);
                OnZoom.Invoke(value);
            }
            _lastZoomDst = currentZommDst;
            _zooming = true;
        }
        else if (!_primaryTouchActive && !_secondaryTouchActive)
        {
            _zooming = false;
        }

        if (!_zooming)
        {
            var primaryDelta = _controlls.Touch.PrimaryTouchDelta.ReadValue<Vector2>();
            OnDeltaChanged?.Invoke(primaryDelta);
            OnTouchPositionChanged?.Invoke(primaryTouchPosition);

            if (primaryTouchPhase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                if (!_isTouchDown)
                {
                    if (_debug) Debug.Log("Touch down");
                    OnTouchStart?.Invoke(primaryTouchPosition);
                    _isTouchDown = true;
                }
            }

            if (primaryTouchPhase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                if (_isTouchDown)
                {
                    if (_debug) Debug.Log("Touch up");
                    OnTouchEnd?.Invoke(primaryTouchPosition);
                    _isTouchDown = false;
                }
            }
        }
    }

    private void ReadManualInput()
    {
        if (_controlls.Mouse.Move.IsPressed())
        {
            var vector = _controlls.Mouse.Move.ReadValue<Vector2>();
            var mouseDelta = _controlls.Mouse.MouseDelta.ReadValue<Vector2>();
            OnDeltaChanged?.Invoke(mouseDelta);
            OnManualInput?.Invoke(vector);
        }
    }
}