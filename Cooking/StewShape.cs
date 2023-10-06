using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class StewShape : MonoBehaviour
{
    [SerializeField] private float _radiusBonus;
    [SerializeField] private LayerMask _potLayer;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _stewBase;
    [SerializeField] private Liquid _water;
    private List<Ingredient> _ingredients;
    private GameObject _currentStew;

    private float _radius;

    public void AttachIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
        var point = Vector3.zero;
        for (int i = 0; i < 50; i++)
        {
            var randomCircle = Random.insideUnitSphere * (_radius * 0.7f); // - ingredient.GetComponent<MeshCollider>().bounds.max.magnitude);
            point = new Vector3(randomCircle.x, 0, randomCircle.z);
            if (Physics.OverlapSphere(transform.position + point, 0.5f).Length == 0)
            {
                break;
            }
        }
        ingredient.transform.SetParent(transform);
        ingredient.transform.rotation = Random.rotation;
        ingredient.GetComponent<Rigidbody>().isKinematic = true;
        ingredient.transform.localPosition = point;
        var rotationSpeed = Random.Range(0.1f, 0.2f);
        //ingredient.transform.DOLocalRotate(new Vector3(0, 180, 0), 1f / rotationSpeed, RotateMode.Fast).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        _particleSystem.Play();
    }

    public void BuildShape()
    {
    }

    public void Clear()
    {
        _ingredients = new List<Ingredient>();
        Destroy(_currentStew);
        _water.gameObject.SetActive(true);
    }
}