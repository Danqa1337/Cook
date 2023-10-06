using DG.Tweening;
using UnityEngine;
using PathCreation;
using System.Linq;

public class MapObjectMovement : MapObjectComponent
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private float _moveSpeed = 1;

    private void Start()
    {
        var points = _pathCreator.path.localPoints.Select(p => p += transform.localPosition).ToArray();
        var duration = _pathCreator.path.length * _moveSpeed;
        transform.localPosition += _pathCreator.path.localPoints[0];
        transform.DOLocalPath(points, duration, PathType.Linear).SetEase(Ease.Linear).SetLoops(-1);
    }

    protected override void OnPlayerEnter()
    {
        Player.instance.Die();
    }

    protected override void OnPlayerExit()
    {
    }

    protected override void OnPlayerStay()
    {
    }
}