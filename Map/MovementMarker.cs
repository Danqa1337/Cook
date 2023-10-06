using UnityEngine;

public class MovementMarker : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private void Start()
    {
        transform.position = Map.instance.transform.position;
    }

    private void OnEnable()
    {
        Player.OnTargetPositionChanged += OnTargetMoved;
        Player.OnPositionChanged += OnPositionChanged;
    }

    private void OnDisable()
    {
        Player.OnTargetPositionChanged -= OnTargetMoved;
        Player.OnPositionChanged -= OnPositionChanged;
    }

    private void OnTargetMoved(Vector2 newPos)
    {
        transform.position = Map.LocalToWorld(newPos);
    }

    private void OnPositionChanged(Vector2 newPos)
    {
        var positions = new Vector3[] { transform.position, Player.instance.transform.position };
        _lineRenderer.SetPositions(positions);
    }
}