using UnityEngine;

public class Portal : MapObjectComponent
{
    [SerializeField] private Portal _exitPortal;
    [SerializeField] private float _rotationAngle;
    [SerializeField] private bool _mirror;
    private bool _justExited;

    protected override void OnPlayerEnter()
    {
        if (!_justExited && _exitPortal != null)
        {
            _justExited = false;
            _exitPortal.ExitFromHere();
        }
    }

    public void ExitFromHere()
    {
        _justExited = true;
        var targetVector = Player.instance.TargetPosition - Player.instance.LocalPosition;
        if (_mirror) targetVector *= -1;
        var rotatedVector = Quaternion.AngleAxis(_rotationAngle, Vector3.forward) * targetVector;
        var newTargetPos = Player.instance.LocalPosition + rotatedVector.ToVector2();
        Player.instance.SetTargetPosition(newTargetPos);
        Player.instance.MoveToPosition(transform.position, Player.TargetPositionHandleMode.Translate);
    }

    protected override void OnPlayerExit()
    {
        _justExited = false;
    }

    protected override void OnPlayerStay()
    {
    }
}