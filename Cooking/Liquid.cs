using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private float _level;
    [SerializeField] private float _offset;
    [SerializeField] private float _radiusBonus;
    [SerializeField] private float _rotationSpeed = 0.1f;

    [SerializeField] private LayerMask _potLayer;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    protected void Start()
    {
        ChangeLevel(_level);
        transform.DORotate(new Vector3(0, 180, 0), 1 / _rotationSpeed, RotateMode.Fast).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void OnValidate()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        ChangeLevel(_level);
    }

    public void ChangeLevel(float level)
    {
        transform.localPosition = new Vector3(0, level + _offset, 0);
        var hit = new RaycastHit();

        Physics.Raycast(transform.position, Vector3.right, out hit, 100, _potLayer);

        var radius = hit.collider != null ? hit.distance : 5;
        _meshRenderer.sharedMaterial.SetFloat("_Radius", radius + _radiusBonus);
        _level = level;
    }
}