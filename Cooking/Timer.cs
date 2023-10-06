using System;
using System.Collections;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    private float _timeSeconds;
    public float TimeSeconds { get => _timeSeconds; }
    private bool _paused;

    public static event Action<float> OnTimeChanged;

    private void OnEnable()
    {
        Kitchen.OnCookingEnded += Pause;
    }

    private void OnDisable()
    {
        Kitchen.OnCookingEnded -= Pause;
    }

    private void StartOver()
    {
        _timeSeconds = 0;
        _paused = false;
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Continue()
    {
        _paused = false;
    }

    private void Update()
    {
        if (!_paused)
        {
            _timeSeconds += Time.deltaTime;
            OnTimeChanged?.Invoke(_timeSeconds);
        }
    }
}