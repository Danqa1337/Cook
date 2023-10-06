using UnityEngine;

public class SliceTrailSpawner : MonoBehaviour
{
    [SerializeField] private SliceTrail _sliceTrailPrefab;
    [SerializeField] private float _LengthMult = 4;

    private void OnEnable()
    {
        SlashBalde.OnSlash += OnSliced;
    }

    private void OnDisable()
    {
        SlashBalde.OnSlash -= OnSliced;
    }

    private void OnSliced(Vector3 slashPoint, Vector3 slashDirection)
    {
        var start = slashPoint - slashDirection * _LengthMult;
        var end = slashPoint + slashDirection * _LengthMult;
        var sliceTrail = Instantiate(_sliceTrailPrefab.gameObject, start, Quaternion.identity).GetComponent<SliceTrail>();
        sliceTrail.Launch(end);
    }
}