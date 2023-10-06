using DG.Tweening;
using System.Linq;
using UnityEngine;

public class SliceTrail : MonoBehaviour
{
    [SerializeField] private float _moveTime = 0.5f;
    [SerializeField] private TrailRenderer _trailRenderer;

    public void Launch(Vector3 position)
    {
        _trailRenderer.emitting = true;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMove(position, _moveTime));
        //seq.AppendCallback(delegate { Destroy(gameObject); });
        seq.Play();
    }
}