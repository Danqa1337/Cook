using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPoint : MonoBehaviour
{
    public static event Action<CameraFollowPoint> FlowPointChanged;

    public static CameraFollowPoint Active;

    public void Activate()
    {
        Active = this;
        Debug.Log(name + " activated");
        FlowPointChanged?.Invoke(this);
    }
}