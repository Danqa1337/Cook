using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Tilemap _fogMap;
    [SerializeField] private TileBase _clearTile;
    [SerializeField] private TileBase _fogTile;
    [SerializeField] private int _viewDistance;
    [SerializeField] private bool _debug;

    private void OnEnable()
    {
        Player.OnEnteredNewCell += ListenNewCell;
        DataHolder.OnLoaded += OnLoaded;
    }

    private void OnDisable()
    {
        Player.OnEnteredNewCell -= ListenNewCell;
        DataHolder.OnLoaded += OnLoaded;
    }

    private void OnLoaded(DataHolder.SaveData saveData)
    {
        var mapSize = DataHolder.SaveData.MapSize;
        foreach (var item in saveData.MapData)
        {
            var cell = item.Index.ToMapPosition(mapSize, mapSize, -32).ToVector2();
            if (item.IsMaped)
            {
                SetTileClear(cell);
            }
            else
            {
                SetTileFog(cell);
            }
        }
    }

    private void ListenNewCell(Vector2Int cell)
    {
        if (_debug) Debug.Log("Fog cleared");
        var cellsInView = KatabasisUtillsClass.GetTilesInRadius(cell, _viewDistance);
        foreach (var c in cellsInView)
        {
            SetTileClear(c);
        }
    }

    public void SetTileClear(Vector2Int cell)
    {
        _fogMap.SetTile(cell.ToVector3(), _clearTile);
        DataHolder.CurrentData.GetTileData(cell).MarkAsMaped();
    }

    private void SetTileFog(Vector2Int cell)
    {
        _fogMap.SetTile(cell.ToVector3(), _fogTile);
        DataHolder.CurrentData.GetTileData(cell).MarkAsUnmaped();
    }
}