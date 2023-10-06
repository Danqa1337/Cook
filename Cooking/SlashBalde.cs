using System;
using System.Linq;
using UnityEngine;

public class SlashBalde : MonoBehaviour
{
    private Vector3 _lastPosition;
    private bool _supplyShown;
    private bool _active;
    private bool _ready;
    [SerializeField] private float _force = 10;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _coolDown = 0.1f;
    [SerializeField] private float _minMagnitudePerTick = 3;
    [SerializeField] private float _backTracingDistance = 1;
    private float _timeSinceLastSlash;

    public static event Action<Vector3, Vector3> OnSlash;

    private void OnEnable()
    {
        TouchController.OnTouchPositionChanged += Follow;
        TouchController.OnTouchStart += OnTouchStart;
        TouchController.OnTouchEnd += OnTouchEnd;
        SuppliesHandler.OnSupplyShown += OnSupplyShown;
        SuppliesHandler.OnSupplyHidden += OnSupplyHidden;
    }

    private void OnDisable()
    {
        TouchController.OnTouchPositionChanged -= Follow;
        TouchController.OnTouchStart -= OnTouchStart;
        TouchController.OnTouchEnd -= OnTouchEnd;
        SuppliesHandler.OnSupplyShown -= OnSupplyShown;
        SuppliesHandler.OnSupplyHidden -= OnSupplyHidden;
    }

    private void Start()
    {
        _lastPosition = transform.position;
        _trailRenderer.emitting = false;
    }

    private void FixedUpdate()
    {
        if (!_ready)
        {
            _timeSinceLastSlash += Time.deltaTime;
            if (_timeSinceLastSlash > _coolDown)
            {
                _ready = true;
            }
        }

        if (_active && _ready && _supplyShown)
        {
            var sqrMagnitude = (transform.position - _lastPosition).sqrMagnitude;
            if (sqrMagnitude >= _minMagnitudePerTick * _minMagnitudePerTick)
            {
                var slashDirection = (transform.position - _lastPosition).normalized;
                var slashPoint = transform.position;
                var hits = GetHits();
                for (int i = 0; i < Mathf.Min(2, hits.Length); i++)
                {
                    var hit = hits[i];
                    if (hit.collider != null)
                    {
                        //Debug.Log("hit");
                        var sliceble = hit.collider.GetComponent<ISliceble>();
                        if (sliceble != null)
                        {
                            slashPoint = hit.point;
                            var pieces = sliceble.Slice(hit.point, Vector3.Cross(slashDirection, (transform.position - MainCameraController.Position).normalized));
                            if (pieces != null)
                            {
                                foreach (var item in pieces)
                                {
                                    item.gameObject.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere * _force);
                                    item.gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.insideUnitSphere * _force);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Debug.Log("no Hit");
                    }
                }
                _timeSinceLastSlash = 0;
                _ready = false;
                OnSlash?.Invoke(slashPoint, slashDirection);
            }
        }
        _lastPosition = transform.position;
    }

    private void Follow(Vector2 vector2)
    {
        var ray = MainCameraController.Camera.ScreenPointToRay(vector2);
        transform.position = ray.origin + ray.direction * 10;
    }

    private void OnTouchStart(Vector2 position)
    {
        _active = true;
        _lastPosition = transform.position;
    }

    private void OnTouchEnd(Vector2 position)
    {
        _active = false;
    }

    private RaycastHit[] GetHits()
    {
        var direction = (transform.position - _lastPosition).normalized;
        var center = _lastPosition - direction * _backTracingDistance;
        var halfExtents = new Vector3(0.02f, 0.02f, 50);
        var maxDistance = (transform.position - _lastPosition).magnitude;
        var hits = Physics.BoxCastAll(center, halfExtents, direction, Quaternion.identity, 5);

        return hits;
    }

    private void OnSupplyHidden()
    {
        _supplyShown = false;
    }

    private void OnSupplyShown(SupplyName supplyName)
    {
        _supplyShown = true;
    }
}