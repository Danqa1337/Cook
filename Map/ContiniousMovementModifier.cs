using UnityEngine;

public abstract class ContiniousMovementModifier : Effect
{
    protected MapGizmo _mapGizmo;

    protected ContiniousMovementModifier(float lifeTime) : base(lifeTime)
    {
    }

    public override void Destroy()
    {
        base.Destroy();
        if (_mapGizmo != null)
        {
            MonoBehaviour.Destroy(_mapGizmo.gameObject);
        }
    }

    public abstract Vector2 GetDisplacementPerSecond(Vector2 currentTargetPosition, bool inverted);

    public abstract Vector2 GetDisplasementPerTick(bool inverted);
}