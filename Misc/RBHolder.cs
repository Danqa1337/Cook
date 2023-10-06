using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBHolder : MonoBehaviour
{
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    public Chunk Grab()
    {
        var colliders = Physics.OverlapSphere(transform.position + _sphereCollider.center, _sphereCollider.radius);
        if (colliders.Length > 0)
        {
            var collision = colliders[0];
            if (collision.GetComponent<Chunk>() != null)
            {
                var chunk = collision.GetComponent<Chunk>();
                if (chunk.Mass >= Kitchen.instance.MinChunkMass * 2)
                {
                    var rb = collision.gameObject.GetComponent<Rigidbody>();
                    rb.transform.SetParent(transform);
                    rb.isKinematic = true;
                    return chunk;
                }
                else
                {
                    Debug.Log("mass to low");
                }
            }
        }
        Debug.Log("Grab nothing");
        return null;
    }
}