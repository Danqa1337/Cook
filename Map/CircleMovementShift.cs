using System;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovementShift : ContiniousMovementModifier
{
    private Vector2 _center;
    public override EffectName EffectName => EffectName.Orbiting;

    public CircleMovementShift(float lifeTime) : base(lifeTime)
    {
        _center = DataHolder.CurrentData.CurrentTown;
    }

    public override Vector2 GetDisplasementPerTick(bool inverted)
    {
        var targetPosition = Player.instance.TargetPosition;
        return GetDisplacementPerSecond(targetPosition, inverted) * Time.deltaTime;
    }

    public override Vector2 GetDisplacementPerSecond(Vector2 currentTargetPosition, bool inverted)
    {
        if (_center != currentTargetPosition)
        {
            var radiusVector = currentTargetPosition - _center;
            var circleLength = (float)(2 * radiusVector.magnitude * Math.PI);
            var baseDegrees = 2f * (inverted ? -1 : 1);
            var degrees = baseDegrees / circleLength;
            var rotatedRadius = Quaternion.AngleAxis(degrees, Vector3.forward) * radiusVector;
            var modifiedTargetPosition = _center + rotatedRadius.ToVector2();
            var displacement = modifiedTargetPosition - currentTargetPosition;

            return displacement / Time.deltaTime;
        }

        return Vector2.zero;
    }
}