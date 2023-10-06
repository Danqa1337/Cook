using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Chunk : Ingredient, ISliceble, ITouchable
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private ParticleSystem _onDestroyParticles;
    [SerializeField] private ParticleSystem _onBreakParticles;
    [SerializeField] private ParticleSystem _onBurnParticles;
    private Rigidbody _rigidbody;
    private MeshCollider _collider;
    private MeshRenderer _meshRenderer;
    [SerializeField] private float _destroyChunkMaxMass = 0.1f;

    [SerializeField] private float _rejectionJumpForce;
    [SerializeField] private Material _crossSectionMaterial;
    public Rigidbody Rigidbody => _rigidbody;
    public override float Mass => _meshFilter.mesh.CalculateVolume() * transform.lossyScale.Volume();
    public Material CrossSectionMaterial { get => _crossSectionMaterial; set => _crossSectionMaterial = value; }
    public Mesh Mesh { get => _meshFilter.mesh; }
    public Collider Collider { get => _collider; }

    public event Action<ISliceble[]> OnSliced;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<MeshCollider>();
    }

    protected void Start()
    {
        AdjustMesh();
    }

    public void AdjustMesh()
    {
        var mesh = _meshFilter.mesh;
        if (mesh.vertices.Length < 4)
        {
            Destroy(gameObject);
        }
        else
        {
            _collider.sharedMesh = mesh;
        }
    }

    public void Destroy()
    {
        Instantiate(_onDestroyParticles.gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public ISliceble[] Slice(Vector3 position, Vector3 normal)
    {
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        if (_rigidbody.mass < _destroyChunkMaxMass)
        {
            OnSliced?.Invoke(new ISliceble[0]);
            Destroy();
            return null;
        }
        else
        {
            Instantiate(_onBreakParticles.gameObject, transform.position, Quaternion.identity);
            var pieces = ChunkSlicer.Slice(this, position, normal);
            foreach (var item in pieces)
            {
                item.gameObject.GetComponent<Chunk>().AdjustMesh();
            }
            OnSliced?.Invoke(pieces);
            return pieces;
        }
    }

    public float GetValue()
    {
        Instantiate(_onBurnParticles.gameObject, transform.position, Quaternion.identity);

        return _rigidbody.mass * 100;
    }

    public void OnTouch(Vector2 point)
    {
        Debug.Log("touch");
        Slice(point, Vector2.up);
    }

    public override void OnRejected(Crockery crockery)
    {
        base.OnRejected(crockery);
        _collider.enabled = false;
        _rigidbody.AddForce(Vector2.up.Rotate(UnityEngine.Random.Range(-45, 45)) * _rejectionJumpForce);
    }

    public override void OnHeating(float value)
    {
    }

    public override void OnNegative()
    {
        foreach (var item in _meshRenderer.materials)
        {
            item.DOColor(Color.red, "_mainColor", 0.2f).Play().PlayBackwards();
        }
    }
}