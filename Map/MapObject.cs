using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public bool IsActive { get; private set; }

    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; }

    public event Action OnPlayerEnter;

    public event Action OnPlayerExit;

    public event Action OnPlayerStay;

    private void Start()
    {
        IsActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive)
        {
            OnPlayerEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsActive)
        {
            OnPlayerExit?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsActive)
        {
            OnPlayerStay?.Invoke();
        }
    }

    public void Show()
    {
        IsActive = true;
        SpriteRenderer.enabled = true;
    }

    public void Hide()
    {
        IsActive = false;
        SpriteRenderer.enabled = false;
    }
}