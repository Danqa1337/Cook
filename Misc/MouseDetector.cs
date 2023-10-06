using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDetector : MonoBehaviour
{
    public static event Action<bool> OnMouseDetected;

    private void Start()
    {
        OnMouseDetected?.Invoke(Mouse.current != null);
    }
}