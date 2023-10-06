using UnityEngine;

public class CameraControllerUi : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Canvas _buttonsCanvas;

    private void OnEnable()
    {
        MouseDetector.OnMouseDetected += OnMouseDetected;
    }

    private void OnDisable()
    {
        MouseDetector.OnMouseDetected -= OnMouseDetected;
    }

    private void OnMouseDetected(bool detected)
    {
        _buttonsCanvas.enabled = detected;
    }

    public void ZoomOut()
    {
        CameraFollowPoint.Active.transform.position += new Vector3(0, 0, -1 * _speed);
    }

    public void ZoomIn()
    {
        CameraFollowPoint.Active.transform.position += new Vector3(0, 0, 1 * _speed);
    }
}