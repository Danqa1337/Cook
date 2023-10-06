using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEffects : Singleton<PlayerEffects>
{
    [SerializeField] private MapGizmo _mapGizmo;

    public static event Action<Effect> OnEffectAdded;

    public static event Action<Effect> OnEffectRemoved;

    private List<Effect> _effects = new List<Effect>();
    private ContiniousMovementModifier[] _modifiers => _effects.Where(e => e is ContiniousMovementModifier).Select(e => e as ContiniousMovementModifier).ToArray();

    public List<Effect> Effects { get => _effects; }
    public List<EffectName> EffectNames => _effects.Select(e => e.EffectName).ToList();
    private int _gizmoTimer = 0;
    private int _maxGizmoTimer = 50;

    private void OnEnable()
    {
        Player.OnTargetPositionChanged += OnTargetPositionChanged;
        Kitchen.OnCookingEnded += Clear;
    }

    private void OnDisable()
    {
        Player.OnTargetPositionChanged -= OnTargetPositionChanged;
        Kitchen.OnCookingEnded += Clear;
    }

    private void OnTargetPositionChanged(Vector2 vector)
    {
        foreach (var modifier in _modifiers)
        {
            //modifier.UpdateGizmo(vector);
        }
        DrawGizmo(vector);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _effects.Count; i++)
        {
            var effect = _effects[i];
            if (effect.LifeTime == 0)
            {
                _effects.Remove(effect);
                effect.Destroy();
            }

            effect.SpendFixedDeltaTime();
        }
    }

    public Vector2 GetDisplacementFromModifiers()
    {
        var displacement = Vector2.zero;
        bool inverted = EffectNames.Contains(EffectName.Inversion);

        foreach (var item in _modifiers)
        {
            displacement += item.GetDisplasementPerTick(inverted);
        }
        return displacement;
    }

    private void DrawGizmo(Vector2 newTargetPosition)
    {
        bool inverted = EffectNames.Contains(EffectName.Inversion);
        if (_gizmoTimer != 0)
        {
            _gizmoTimer -= 1;
            return;
        }
        _gizmoTimer = _maxGizmoTimer;
        var modifiers = _modifiers;
        if (modifiers.Length > 0)
        {
            var maxTicks = modifiers.Max(m => m.LifeTime) / Time.fixedDeltaTime;

            var points = new List<Vector2>() { newTargetPosition };

            for (int i = 0; i < maxTicks; i++)
            {
                var combinedDisplacement = Vector2.zero;
                foreach (var item in modifiers)
                {
                    if (item.LifeTime / Time.fixedDeltaTime >= i)
                    {
                        combinedDisplacement += item.GetDisplacementPerSecond(newTargetPosition, inverted) * Time.fixedDeltaTime;
                    }
                }
                newTargetPosition += combinedDisplacement;

                points.Add(newTargetPosition);
            }
            _mapGizmo.Draw(points.ToArray());
        }
        else
        {
            _mapGizmo.Draw(new Vector2[0]);
        }
    }

    public void AddEffect(Effect effect)
    {
        var existingEffect = _effects.FirstOrDefault(m => m.EffectName == effect.EffectName);

        Debug.Log("Effect added for " + effect.LifeTime + " seconds");
        if (existingEffect != null)
        {
            existingEffect.AddLifeTime(effect.LifeTime);
        }
        else
        {
            effect.Start();
            _effects.Add(effect);
            OnEffectAdded?.Invoke(effect);
        }
    }

    public void Clear()
    {
        foreach (var item in _effects)
        {
            item.Destroy();
        }
        _effects.Clear();
    }
}