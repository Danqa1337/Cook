using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragContoller : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _mapTransform;
    [SerializeField] private MapDragHandler _mapDragHandler;

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}

public class MapDragHandler : MonoBehaviour
{
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
}