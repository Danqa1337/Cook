using System;
using UnityEngine;

public interface ISliceble
{
    public GameObject gameObject { get; }
    public Transform transform { get; }
    public Collider Collider { get; }

    public event Action<ISliceble[]> OnSliced;

    public ISliceble[] Slice(Vector3 position, Vector3 normal);
}