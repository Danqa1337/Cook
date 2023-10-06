using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    [SerializeField] private Label _label;

    private void OnEnable()
    {
        Timer.OnTimeChanged += ShowTime;
    }

    private void OnDisable()
    {
        Timer.OnTimeChanged -= ShowTime;
    }

    private void ShowTime(float timeSeconds)
    {
        _label.SetValue(Timer.instance.TimeSeconds);
    }
}