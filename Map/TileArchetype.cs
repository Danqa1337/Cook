using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileArchetype", menuName = "Map/TileArchetype")]
public class TileArchetype : ScriptableObject
{
    [SerializeField] private Biome _biome;
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private IngredientName[] _naturalIngredients;
    [SerializeField] private TileBase[] _tiles;

    public TileBase[] Tiles { get => _tiles; }
    public IngredientName[] NaturalIngredients { get => _naturalIngredients; }
    public float MoveSpeed { get => _moveSpeed; }
    public Biome Biome { get => _biome; }
}