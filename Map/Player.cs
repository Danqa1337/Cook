using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Singleton<Player>, IGameStateChangeHandler
{
    [SerializeField] private float _checkDistance = 0.1f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _movementScale;

    private bool _controlled;
    private Vector2 _targetPosition;
    public float MovementScale { get => _movementScale; }
    public Vector2 LocalPosition => transform.localPosition;

    public Vector2 TargetPosition { get => _targetPosition; set => _targetPosition = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    public static event Action<Vector2Int> OnEnteredNewCell;

    public static event Action<Vector2> OnPositionChanged;

    public static event Action<Vector2> OnTargetPositionChanged;

    public static event Action OnDied;

    private Vector2Int _currentCell;
    private bool _onTheGround = true;

    public enum TargetPositionHandleMode
    {
        Reset,
        Keep,
        Translate,
    }

    private void OnEnable()
    {
        Kitchen.instance.ActiveCrockery.OnIngredientAdded += ListenIngredient;
        TouchController.OnManualInput += ManulaMove;
        DataHolder.OnLoaded += OnLoaded;
        Kitchen.OnCookingEnded += ReturnToTown;
    }

    private void OnDisable()
    {
        Kitchen.instance.ActiveCrockery.OnIngredientAdded -= ListenIngredient;
        TouchController.OnManualInput -= ManulaMove;
        DataHolder.OnLoaded -= OnLoaded;
        Kitchen.OnCookingEnded -= ReturnToTown;
    }

    private void Start()
    {
        SetPosition(Map.BaseSpawnPoint.position);
        _currentCell = Map.GetCell(transform.position);
        SetTargetPosition(transform.position.ToVector2());
    }

    public void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Cooking:

                _controlled = true;
                OnEnteredNewCell?.Invoke(Map.GetCell(transform.position));
                break;

            case GameState.Town:
                Stop();
                goto default;
            default:
                _controlled = false;
                break;
        }
    }

    private void ListenIngredient(Ingredient ingredient)
    {
        var inverted = PlayerEffects.instance.EffectNames.Contains(EffectName.Inversion);
        var inversionMult = inverted ? -1 : 1;
        switch (ingredient.StaticIngredientData.effect)
        {
            case EffectName.Movement:
                MoveTargetPosition(ingredient.StaticIngredientData.movement * ingredient.Mass * _movementScale * inversionMult);
                break;

            case EffectName.Jump:
                Jump(ingredient.StaticIngredientData.movement * ingredient.Mass * _movementScale * inversionMult);
                break;

            case EffectName.SwimingAbility:
                PlayerEffects.instance.AddEffect(new AbilityToSwimEffect(-1));
                break;

            case EffectName.Orbiting:
                PlayerEffects.instance.AddEffect(new CircleMovementShift(ingredient.Mass * _movementScale * ingredient.StaticIngredientData.power));
                break;

            case EffectName.Shift:
                PlayerEffects.instance.AddEffect(new VectorMovementModifier(ingredient.Mass * _movementScale, ingredient.StaticIngredientData.movement * ingredient.StaticIngredientData.power));
                break;

            case EffectName.SpeedUpTime:
                if (!inverted)
                {
                    PlayerEffects.instance.AddEffect(new SpeedTimeEffect(ingredient.Mass * _movementScale * ingredient.StaticIngredientData.power));
                }
                else
                {
                    PlayerEffects.instance.AddEffect(new SlowTimeEffect(ingredient.Mass * _movementScale * ingredient.StaticIngredientData.power));
                }

                break;

            case EffectName.SlowDownTime:
                if (!inverted)
                {
                    PlayerEffects.instance.AddEffect(new SlowTimeEffect(ingredient.Mass * _movementScale * ingredient.StaticIngredientData.power));
                }
                else
                {
                    PlayerEffects.instance.AddEffect(new SpeedTimeEffect(ingredient.Mass * _movementScale * ingredient.StaticIngredientData.power));
                }
                break;

            case EffectName.Inversion:
                PlayerEffects.instance.AddEffect(new InversionEffect(ingredient.Mass * _movementScale * ingredient.StaticIngredientData.power));
                break;

            default:
                break;
        }
    }

    public void SetTargetPosition(Vector2 newTargetPosition)
    {
        _targetPosition = newTargetPosition;
        OnTargetPositionChanged?.Invoke(_targetPosition);
    }

    private void ManulaMove(Vector2 vector2)
    {
        MoveTargetPosition(vector2 * Time.fixedDeltaTime);
    }

    private void OnLoaded(DataHolder.SaveData saveData)
    {
        MoveToPosition(saveData.PlayerPosition);
    }

    private void ReturnToTown()
    {
        MoveToPosition(DataHolder.CurrentData.CurrentTown);
    }

    public void Stop()
    {
        SetTargetPosition(transform.position);
    }

    public void MoveToPosition(Vector2 position, TargetPositionHandleMode targetPositionHandleMode = TargetPositionHandleMode.Reset)
    {
        var targetVector = TargetPosition - LocalPosition;
        SetPosition(position);
        switch (targetPositionHandleMode)
        {
            case TargetPositionHandleMode.Reset:
                SetTargetPosition(Map.WorldToLocal(transform.position));

                break;

            case TargetPositionHandleMode.Keep:
                break;

            case TargetPositionHandleMode.Translate:
                SetTargetPosition(LocalPosition + targetVector);
                break;

            default:
                break;
        }
    }

    private void MoveTargetPosition(Vector2 displacement)
    {
        SetTargetPosition(_targetPosition + displacement);
    }

    private void Jump(Vector2 displacement)
    {
        if (_onTheGround)
        {
            Debug.Log("Jump");
            var magnitude = displacement.magnitude;
            var duration = magnitude;
            _onTheGround = false;
            transform.DOPunchScale(Vector3.one * Mathf.Max(magnitude, 2), duration, 0, 0);
            var seq = DOTween.Sequence();
            seq.Append(transform.DOLocalJump(LocalPosition + displacement, 1, 1, duration));
            seq.AppendCallback(delegate
            {
                _onTheGround = true;
                SetTargetPosition(Map.WorldToLocal(transform.position));
            });
            seq.Play();
        }
    }

    private void FixedUpdate()
    {
        if (GameStateManager.CurrentGameState == GameState.Cooking && _onTheGround)
        {
            SetTargetPosition(_targetPosition + PlayerEffects.instance.GetDisplacementFromModifiers());
            var targetVector = (_targetPosition - LocalPosition);
            var distance = targetVector.magnitude;
            var currentCell = Map.GetCell(transform.position.ToVector2());

            if (currentCell != _currentCell)
            {
                _currentCell = currentCell;
                OnEnteredNewCell?.Invoke(_currentCell);
            }

            var direction = targetVector.normalized;

            if (CheckWalk(direction))
            {
                if (distance > 0.1)
                {
                    var speedMult = TilesDatabase.GetTileArchetype(Map.GetTile(transform.position)).MoveSpeed;
                    var newLocalPos = LocalPosition + (direction * Time.fixedDeltaTime * _moveSpeed * speedMult);
                    SetPosition(newLocalPos);
                }
                else
                {
                    SetPosition(_targetPosition);
                }
            }
            else
            {
                SetTargetPosition(LocalPosition);
            }
        }
    }

    private void SetPosition(Vector2 position)
    {
        transform.localPosition = position;
        OnPositionChanged?.Invoke(position);
        DataHolder.CurrentData.SetPlayerPosition(position);
    }

    public void Die()
    {
        Debug.Log("Died");
        OnDied?.Invoke();
        ReturnToTown();
    }

    private bool CheckWalk(Vector2 direction)
    {
        var worldPoint = LocalPosition + direction * _checkDistance;
        var tileBase = Map.GetTile(worldPoint);

        return IsWalkable(tileBase);
    }

    private bool IsWalkable(TileBase tileBase)
    {
        var arch = TilesDatabase.GetTileArchetype(tileBase);
        var canSwim = PlayerEffects.instance.EffectNames.Contains(EffectName.SwimingAbility);

        switch (arch.Biome)
        {
            case Biome.Meadow:
                return true;

            case Biome.Forest:
                return true;

            case Biome.Swamp:
                return true;

            case Biome.Sea:
                return canSwim;

            case Biome.River:
                return canSwim;

            case Biome.Mountain:
                return false;

            case Biome.Field:
                return true;

            default: return false;
        }
    }
}