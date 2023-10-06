using UnityEngine;

public class MapScroller : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private RectTransform _mapRect;
    private bool _active;
    private bool _touchesMap;

    private void OnEnable()
    {
        SuppliesHandler.OnSupplyHidden += OnSupplyHidden;
        SuppliesHandler.OnSupplyShown += OnSupplyShown;
        TouchController.OnDeltaChanged += OnDelta;
        TouchController.OnTouchPositionChanged += OnTouchPositionChanged;
    }

    private void OnDisable()
    {
        SuppliesHandler.OnSupplyHidden -= OnSupplyHidden;
        SuppliesHandler.OnSupplyShown -= OnSupplyShown;
        TouchController.OnDeltaChanged -= OnDelta;
        TouchController.OnTouchPositionChanged += OnTouchPositionChanged;
    }

    private void OnSupplyHidden()
    {
        _active = true;
    }

    private void OnSupplyShown(SupplyName supplyName)
    {
        _active = false;
        CameraFollowPoint.Active.transform.localPosition = new Vector3(0, 0, CameraFollowPoint.Active.transform.localPosition.z);
    }

    private void OnDelta(Vector2 vector)
    {
        if (_touchesMap && _active && GameStateManager.CurrentGameState == GameState.Cooking)
        {
            var zMult = CameraFollowPoint.Active.transform.localPosition.z / 18f;
            CameraFollowPoint.Active.transform.localPosition += vector.ToVector3() * Time.fixedDeltaTime * _speed * zMult;
        }
    }

    private void OnTouchPositionChanged(Vector2 position)
    {
        _touchesMap = IsPointInRT(position, _mapRect);
    }

    private bool IsPointInRT(Vector2 point, RectTransform rt)
    {
        // Get the rectangular bounding box of your UI element
        Rect rect = rt.rect;

        // Get the left, right, top, and bottom boundaries of the rect
        float leftSide = rt.anchoredPosition.x - rect.width / 2;
        float rightSide = rt.anchoredPosition.x + rect.width / 2;
        float topSide = rt.anchoredPosition.y + rect.height / 2;
        float bottomSide = rt.anchoredPosition.y - rect.height / 2;

        //Debug.Log(leftSide + ", " + rightSide + ", " + topSide + ", " + bottomSide);

        // Check to see if the point is in the calculated bounds
        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
        {
            return false;
        }
        return true;
    }
}