using UnityEngine;

public class CameraPositionsManager : MonoBehaviour
{
    [SerializeField] private CameraFollowPoint _cookingPoint;
    [SerializeField] private CameraFollowPoint _orderPoint;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void ActivateOrderPoint()
    {
        _orderPoint.Activate();
    }

    private void ActivateCookingPoint()
    {
        _cookingPoint.Activate();
    }
}