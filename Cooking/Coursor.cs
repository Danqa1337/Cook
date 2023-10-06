using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coursor : MonoBehaviour
{
    public UnityEvent<Ray> OnPointerDownEvent;
    public UnityEvent<Ray> OnPointerUpEvent;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
}