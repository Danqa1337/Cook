using System;
using UnityEngine;

public class SliceComboManager : MonoBehaviour
{
    [SerializeField] private float _maxComboSpan = 1f;
    private float _currentComboSpan;
    private int _currentComboLength;

    public static event Action<int> OnComboEnded;

    public static event Action<int, Vector2> OnComboIncreased;

    public static event Action<int, Vector2> OnComboStarted;

    private void OnEnable()
    {
        ChunkSlicer.OnSliced += OnSliced;
    }

    private void OnDisable()
    {
        ChunkSlicer.OnSliced -= OnSliced;
    }

    private void OnSliced(ISliceble[] pieces, Vector3 position, Vector3 normal)
    {
        _currentComboSpan = 0;
        _currentComboLength++;
        if (_currentComboLength > 1)
        {
            if (_currentComboLength == 2)
            {
                Debug.Log("Combo started");
                OnComboStarted?.Invoke(_currentComboLength, MainCameraController.Camera.WorldToScreenPoint(position).ToVector2());
            }
            OnComboIncreased?.Invoke(_currentComboLength, MainCameraController.Camera.WorldToScreenPoint(position).ToVector2());
        }
    }

    private void Update()
    {
        if (_currentComboLength > 0)
        {
            _currentComboSpan += Time.deltaTime;
            if (_currentComboSpan > _maxComboSpan)
            {
                EndCombo();
            }
        }
    }

    private void EndCombo()
    {
        if (_currentComboLength > 1)
        {
            OnComboEnded?.Invoke(_currentComboLength);
            Debug.Log("Combo ended");
        }
        _currentComboSpan = 0;
        _currentComboLength = 0;
    }
}