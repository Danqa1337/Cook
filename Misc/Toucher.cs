using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Toucher : MonoBehaviour
{
    [SerializeField] private bool _debug;
    [SerializeField] private float _radius = 0.5f;

    private void OnEnable()
    {
        TouchController.OnTouchStart += Touch;
    }

    private void OnDisable()
    {
        TouchController.OnTouchStart -= Touch;
    }

    public void Touch(Vector2 screenPosition)
    {
        var hit = new RaycastHit();
        Physics.Raycast(MainCameraController.Camera.ScreenPointToRay(screenPosition), out hit);
        if (hit.collider != null)
        {
            var usable = hit.collider.gameObject.GetComponent<IUsable>();
            if (usable != null)
            {
                Debug.Log("Use");
                usable.OnUse();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}