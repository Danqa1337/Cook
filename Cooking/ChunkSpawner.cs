using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public static Action<Chunk> OnChunkSpawned;
    [SerializeField] private float _spawnIntervalSeconds = 2;

    [Min(3)]
    [SerializeField] private int _minChunkSidesNum;

    [SerializeField] private int _maxChunkSidesNum;
    [SerializeField] private float _minChunkSize;
    [SerializeField] private float _maxChunkSize;
    [SerializeField] private float _chunkFormDeviation;

    private EdgeCollider2D _spawnArea;

    private void Awake()
    {
        _spawnArea = GetComponent<EdgeCollider2D>();
    }

    private void Start()
    {
        SpawnNewChunk();
        StartCoroutine(DoSpawnLoop());
    }

    private IEnumerator DoSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnIntervalSeconds);
            SpawnNewChunk();
        }
    }

    private void SpawnNewChunk()
    {
        var newChunk = ChunkFactory.CreateChunk(UnityEngine.Random.Range(_minChunkSidesNum, _maxChunkSidesNum), _chunkFormDeviation, UnityEngine.Random.Range(_minChunkSize, _maxChunkSize));
        newChunk.transform.rotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)));
        newChunk.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-_spawnArea.bounds.extents.x, _spawnArea.bounds.extents.x), 0, 0);
        OnChunkSpawned?.Invoke(newChunk.GetComponent<Chunk>());
    }
}

public class ChunkSpawnerState
{
    public virtual void Update()
    {
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void SpawnChunk()
    {
    }

    protected virtual void GetSpawnPoint()
    {
    }
}