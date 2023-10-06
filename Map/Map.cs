using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Biome
{
    Meadow,
    Forest,
    Swamp,
    Sea,
    River,
    Mountain,
    Desert,
    Field,
}

public class Map : Singleton<Map>
{
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Transform _baseSpawnPoint;
    [SerializeField] private MapGizmo _mapGizmoPrefab;
    public MapSite[] Sites => GetComponentsInChildren<MapSite>();
    public static Transform BaseSpawnPoint { get => instance._baseSpawnPoint; }
    public MapGizmo MapGizmoPrefab { get => _mapGizmoPrefab; }

    public static TileBase GetTile(Vector2 worldPosition)
    {
        var cell = GetCell(worldPosition);
        var tile = instance._tileMap.GetTile(cell.ToVector3());
        return tile;
    }

    public MapGizmo SpawnMapGizmo()
    {
        var gizmo = Instantiate(_mapGizmoPrefab.gameObject, _tileMap.transform).GetComponent<MapGizmo>();
        return gizmo;
    }

    public void Spawn(GameObject gameObject, Vector2 localPopsition)
    {
        gameObject.transform.SetParent(_tileMap.transform);
        gameObject.transform.localPosition = localPopsition;
    }

    public static Vector2Int GetCell(Vector2 worldPosition)
    {
        return instance._tileMap.WorldToCell(worldPosition).ToVector2();
    }

    public static Vector2 LocalToWorld(Vector2 localPosition)
    {
        return instance._tileMap.transform.position.ToVector2() + localPosition;
    }

    public static Vector2 WorldToLocal(Vector2 worldPosition)
    {
        return instance._tileMap.WorldToLocal(worldPosition).ToVector2();
    }
}