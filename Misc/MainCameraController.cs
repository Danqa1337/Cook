using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class MainCameraController : Singleton<MainCameraController>
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _followPoint;
    private Camera _camera;
    public static Camera Camera => instance._camera;
    public static Vector3 Position => instance.transform.position;
    public static Quaternion Rotation => instance.transform.rotation;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        CameraFollowPoint.FlowPointChanged += OnFPChanged;
        TouchController.OnZoom += OnZoom;
    }

    private void OnDisable()
    {
        CameraFollowPoint.FlowPointChanged -= OnFPChanged;
        TouchController.OnZoom -= OnZoom;
    }

    private void StartFollow()
    {
        _followPoint = true;
        OnFPChanged(CameraFollowPoint.Active);
    }

    private void OnZoom(float value)
    {
        if (GameStateManager.CurrentGameState == GameState.Cooking)
        {
            var currentPos = CameraFollowPoint.Active.transform.localPosition;
            var distance = Mathf.Clamp(currentPos.z - value * 0.02f, -50, -15);
            CameraFollowPoint.Active.transform.localPosition = new Vector3(currentPos.x, currentPos.y, distance);
        }
    }

    private void StopFollow()
    {
        _followPoint = false;
    }

    private void OnFPChanged(CameraFollowPoint cameraFollowPoint)
    {
        if (_followPoint)
        {
            // transform.position = cameraFollowPoint.transform.position;

            transform.DOKill();
            var time = Mathf.Min(2f, 1f / _moveSpeed * (cameraFollowPoint.transform.position - transform.position).sqrMagnitude);
            transform.SetParent(cameraFollowPoint.transform);
            transform.DOLocalMove(Vector2.zero, time);

            transform.DORotate(cameraFollowPoint.transform.rotation.eulerAngles, time);
        }
    }
}