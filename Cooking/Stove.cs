using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [SerializeField] private float _temperature = 0;
    [SerializeField] private ParticleSystem _smoke;
    public float Temperature { get => _temperature; }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void ChangeTemperature(float newTemp)
    {
        _temperature = Mathf.Clamp(newTemp, 0, 100);
        var emission = _smoke.emission;
        //emission.rate = 10 * _temperature;
    }

    private void Clear()
    {
        ChangeTemperature(0);
    }
}