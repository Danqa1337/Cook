using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UiCanvas : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private UIName _uiName;
    [SerializeField] private CameraFollowPoint _cameraFollowPoint;

    public UIName UiName { get => _uiName; }

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    public virtual void Show()
    {
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        if (_cameraFollowPoint != null)
        {
            _cameraFollowPoint.Activate();
        }
    }

    public virtual void Hide()
    {
        _canvas.renderMode = RenderMode.WorldSpace;
        transform.position = new Vector3(-1000, -1000);
        transform.parent.GetComponent<LayoutGroup>().CalculateLayoutInputVertical();
        transform.parent.GetComponent<LayoutGroup>().CalculateLayoutInputHorizontal();
    }
}