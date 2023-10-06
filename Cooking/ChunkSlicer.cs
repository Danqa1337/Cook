using EzySlice;
using System;
using System.Linq;
using UnityEngine;

public class ChunkSlicer
{
    public static Action<ISliceble[], Vector3, Vector3> OnSliced;

    public static ISliceble[] Slice(Chunk chunk, Vector3 position, Vector3 normal)
    {
        var result = new ISliceble[] { chunk };
        if (chunk.Mass >= Kitchen.instance.MinChunkMass)
        {
            var crossectionMaterial = chunk.CrossSectionMaterial;
            var pices = chunk.gameObject.SliceInstantiate(position, normal, new TextureRegion(0, 0, 1, 1), crossectionMaterial, ChunkFactory.ChunkPrefab);
            if (pices != null && pices.Length > 1)
            {
                for (int i = 0; i < pices.Length; i++)
                {
                    Physics.SyncTransforms();
                    var piece = pices[i];
                    piece.layer = chunk.gameObject.layer;
                    piece.name = chunk.gameObject.name + " piece";
                    piece.GetComponent<Chunk>().AdjustMesh();
                    piece.GetComponent<Rigidbody>().isKinematic = false;
                }
                MonoBehaviour.Destroy(chunk.gameObject);

                result = pices.Select(p => p.gameObject.GetComponent<ISliceble>()).ToArray();
                Debug.Log("Sliced");
                OnSliced?.Invoke(result, position, normal);
            }
        }
        return result;
    }
}

public class ChunkSlicer3D
{
    public static GameObject[] Slice(Chunk chunk, Vector3 position, Vector3 normal)
    {
        if (true)//chunk.Mass > Kitchen.MinChunkMass)
        {
            var crossectionMaterial = chunk.GetComponent<MeshRenderer>().material;

            var pices = chunk.gameObject.SliceInstantiate(position, normal, new TextureRegion(0, 0, 1, 1), crossectionMaterial, ChunkFactory.ChunkPrefab);
            if (pices != null && pices.Length > 1)
            {
                if (true)// pices.All(p => p.GetComponent<Chunk>().Mass >= Kitchen.MinChunkMass))
                {
                    for (int i = 0; i < pices.Length; i++)
                    {
                        Physics2D.SyncTransforms();
                        var piece = pices[i];
                        piece.layer = chunk.gameObject.layer;
                        piece.name = chunk.gameObject.name + " piece";
                        piece.GetComponent<Chunk>().AdjustMesh();
                        piece.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    }
                    MonoBehaviour.Destroy(chunk.gameObject);
                }
                else
                {
                    foreach (var item in pices)
                    {
                        MonoBehaviour.Destroy(item);
                    }
                }
            }
            return pices;
        }
        return new GameObject[] { chunk.gameObject };
    }
}