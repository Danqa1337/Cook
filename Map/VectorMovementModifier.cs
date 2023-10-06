using UnityEngine;

public class VectorMovementModifier : ContiniousMovementModifier
{
    public readonly Vector2 Vector;

    public override EffectName EffectName => EffectName.Shift;

    public VectorMovementModifier(float lifeTime, Vector2 vector) : base(lifeTime)
    {
        Vector = vector;
    }

    public override Vector2 GetDisplasementPerTick(bool inverted)
    {
        return GetDisplacementPerSecond(Vector2.zero, inverted) * Time.fixedDeltaTime;
    }

    public override Vector2 GetDisplacementPerSecond(Vector2 currentTargetPosition, bool inverted)
    {
        return Vector * (inverted ? -1 : 1);
    }
}