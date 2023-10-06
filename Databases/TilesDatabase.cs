using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TilesDatabase", menuName = "Databases/TilesDatabase")]
public class TilesDatabase : Database<TilesDatabase, TilesDatabase, TilesDatabase, TilesDatabase>
{
    [SerializeField] private TileArchetype[] _archetypes;
    private SerializableDictionary<TileBase, TileArchetype> _archetypesByTile;

    public static TileArchetype GetTileArchetype(TileBase tileBase)
    {
        return instance._archetypesByTile[tileBase];
    }

    public override void StartUp()
    {
        _archetypesByTile = new SerializableDictionary<TileBase, TileArchetype>();
        foreach (var archetype in _archetypes)
        {
            foreach (var tile in archetype.Tiles)
            {
                _archetypesByTile.Add(tile, archetype);
            }
        }
    }

    protected override void ProcessParam(TilesDatabase param)
    {
    }
}