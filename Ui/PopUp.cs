using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Label _label;
    [Min(0.1f)][SerializeField] private float _speed = 3;

    public void Draw(string text, Color color)
    {
        _label.SetText(text);
        _label.SetColor(color);
        transform.DOMove(transform.position + (Vector3.up * 3) + UnityEngine.Random.insideUnitSphere, 1f / _speed).OnComplete(new TweenCallback(delegate { Destroy(gameObject); }));
    }
}