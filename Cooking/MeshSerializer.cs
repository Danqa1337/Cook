using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Mathematics;
using System.Linq;

[Serializable]
public class MeshData
{
    public float3[] vertices;
    public float2[] UV1;
    public float2[] UV2;

    public int[] triangels;

    public string name;
}

public static class MeshExtensions
{
    public static void SetMeshData(this Mesh mesh, MeshData meshData)
    {
        mesh.Clear();
        Debug.Log(meshData.vertices.Length);
        mesh.SetVertices(meshData.vertices.Select(v => v.ToVector3()).ToArray());
        mesh.SetTriangles(meshData.triangels, 0);
        mesh.SetUVs(0, meshData.UV1.Select(v => v.ToVector2()).ToArray());
        mesh.SetUVs(1, meshData.UV2.Select(v => v.ToVector2()).ToArray());

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    public static MeshData Serialize(this Mesh mesh)
    {
        if (mesh == null) throw new NullReferenceException("mesh is null");

        var meshdata = new MeshData();
        meshdata.vertices = mesh.vertices.Select(v => v.ToFloat3()).ToArray();
        meshdata.triangels = mesh.triangles;
        var colors = new List<Color>();
        mesh.GetColors(colors);
        meshdata.UV1 = mesh.uv.Select(v => v.ToFloat2()).ToArray();
        meshdata.UV2 = mesh.uv2.Select(v => v.ToFloat2()).ToArray();

        meshdata.name = mesh.name;
        return meshdata;
    }
}