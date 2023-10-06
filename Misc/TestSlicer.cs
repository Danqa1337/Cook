using EzySlice;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestSlicer : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var chunks = FindObjectsOfType<Chunk>();
            
            foreach (var chunk in chunks)
            {

                var pices = chunk.gameObject.SliceInstantiate(new EzySlice.Plane(chunk.gameObject.transform.position, Random.insideUnitCircle.ToVector3()), new TextureRegion(0, 0, 1, 1), chunk.GetComponent<MeshRenderer>().material);
                if (pices != null)
                {
                    foreach (var pice in pices)
                    {
                        pice.AddComponent<Chunk>();
                        pice.AddComponent<Rigidbody2D>();
                        var collider = pice.AddComponent<PolygonCollider2D>();
                        var mesh = pice.GetComponent<MeshFilter>().mesh;
                        mesh.Optimize();
                        PolygonColiderCreator.SetColliderPoints(mesh, collider);


                    }
                    Destroy(chunk.gameObject);
                }
            }
        }
    }
}
