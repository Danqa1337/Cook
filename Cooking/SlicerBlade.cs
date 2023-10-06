using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerBlade : MonoBehaviour
{
    private bool _activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_activated)
        {
            var sliceble = collision.gameObject.GetComponent<ISliceble>();
            if (sliceble != null)
            {
                var pices = sliceble.Slice(transform.position, transform.right);
                if (pices != null && pices.Length > 1)
                {
                    foreach (var item in pices)
                    {
                        var pushVector = item.gameObject.GetComponent<Collider2D>().bounds.center.y < transform.position.y ? Vector2.down : Vector2.up;
                        item.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector * 100);
                    }
                }
            }
        }
    }

    public void Activate()
    {
        _activated = true;
    }

    public void Deactivate()
    {
        _activated = false;
    }
}