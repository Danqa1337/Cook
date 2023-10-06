using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Misc,
    Meat,
    Veg,
}

public enum EffectName
{
    Movement,
    Jump,
    SwimingAbility,
    Orbiting,
    Shift,
    SpeedUpTime,
    SlowDownTime,
    Inversion,
    Stop,
}

[System.Serializable]
public struct StaticIngredientData
{
    public float score;
    public EffectName effect;
    public Vector2 movement;
    public float power;
    public IngredientType ingredientType;
    public IngredientName[] sinergies;
    public IngredientName[] dissinergies;
}