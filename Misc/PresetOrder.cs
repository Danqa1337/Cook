using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Preset Order")]
public class PresetOrder : ScriptableObject
{
    [SerializeField] private Order _order = new Order();
    public Order Order { get => _order; set => _order = value; }
}

[Serializable]
public class PresetOrderData
{
    public bool Completed;
    public AppriceResult AppriceResult = AppriceResult.Null;
}