using UnityEngine;

public class MovementRendererAligner : MonoBehaviour
{
    [SerializeField] private Transform _transformToAlign;
    private float _lastXPos;
    private Vector3 _baseScale;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    private void Update()
    {
        var currentXPos = transform.localPosition.x;

        if (_lastXPos - currentXPos > 0)
        {
            transform.localScale = _baseScale;
        }
        else
        {
            transform.localScale = new Vector3(_baseScale.x * -1, _baseScale.y, _baseScale.z);
        }
        _lastXPos = currentXPos;
    }
}