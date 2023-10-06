using System;
using System.Linq;
using UnityEngine;

public class ChunkSupply : Supply
{
    [SerializeField] private Transform _chunkHolder;
    private Chunk _chunk;
    private float _defaultMass;

    public override event Action<SupplyName, float> OnRemainingChanged;

    public override event Action<SupplyName> OnEmpty;

    public override float Remaining => _chunk != null ? _chunk.Mass / _defaultMass : 0;

    public Chunk Chunk { get => _chunk; }

    public override StaticIngredientData IngData => _ingredient.StaticIngredientData;

    private void Awake()
    {
        _chunk = _chunkHolder.GetComponentInChildren<Chunk>();
        _defaultMass = _chunk.Mass;
    }

    public override void OnSpawned(SupplyData supplyData)
    {
        if (supplyData.MeshData != null && supplyData.remaining != 1)
        {
            _chunk.Mesh.SetMeshData(supplyData.MeshData);
            _chunk.AdjustMesh();
        }

        UpdateChunk(_chunk);
    }

    private void UpdateChunk(Chunk chunk)
    {
        if (_chunk != null)
        {
            _chunk.OnSliced -= OnChunkSliced;
        }
        _chunk = chunk;

        _chunk.transform.SetParent(_chunkHolder.transform);
        _chunk.Rigidbody.isKinematic = true;
        OnRemainingChanged?.Invoke(_supplyName, Remaining);
        _chunk.OnSliced += OnChunkSliced;
    }

    private void OnChunkSliced(ISliceble[] pieces)
    {
        if (pieces.Length > 0)
        {
            var chunk = pieces.OrderBy(p => (p.Collider.bounds.center - _chunkHolder.transform.position).magnitude).ToArray()[0] as Chunk;
            if (chunk.Mass >= Kitchen.instance.MinChunkMass * 2)
            {
                UpdateChunk(chunk);
            }
            else
            {
                Debug.Log("Supply " + _supplyName + " is empty");
                OnEmpty?.Invoke(_supplyName);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_chunkHolder.transform.position, 0.1f);
    }

    public override SupplyData GetSupplyData()
    {
        var data = base.GetSupplyData();
        data.MeshData = _chunk.Mesh.Serialize();
        return data;
    }
}