using UnityEngine;

[RequireComponent(typeof(MapObject))]
public abstract class MapObjectComponent : MonoBehaviour
{
    protected MapObject _mapObject;

    protected virtual void Awake()
    {
        _mapObject = GetComponent<MapObject>();
        _mapObject.OnPlayerEnter += OnPlayerEnter;
        _mapObject.OnPlayerExit += OnPlayerExit;
        _mapObject.OnPlayerStay += OnPlayerStay;
    }

    protected abstract void OnPlayerEnter();

    protected abstract void OnPlayerExit();

    protected abstract void OnPlayerStay();
}