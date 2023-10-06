using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuppliesHandler : Singleton<SuppliesHandler>, IGameStateChangeHandler
{
    [SerializeField] private Transform _activeHolder;
    [SerializeField] private Transform _inactiveHolder;
    private Vector3 _activeHolderPosition;

    private List<Supply> _spawnedSupplies = new List<Supply>();
    private Supply _shownSupply;

    public static event Action<SupplyName> OnOutOfSupply;

    public static event Action<SupplyName, float> OnSupplyRemainingChanged;

    public static event Action<SupplyName, int> OnSupplyQuantityChanged;

    public static event Action<SupplyName> OnSupplyShown;

    public static event Action OnSupplyHidden;

    private void OnEnable()
    {
        Kitchen.OnCookingEnded += OnCookingEnded;
        Kitchen.OnCookingEnded += OnCookingStarted;
    }

    private void OnDisable()
    {
        Kitchen.OnCookingEnded -= OnCookingEnded;
        Kitchen.OnCookingEnded -= OnCookingStarted;
    }

    private void Awake()
    {
        _activeHolderPosition = _activeHolder.localPosition;
    }

    private void OnCookingStarted()
    {
        Hide();
    }

    private void OnCookingEnded()
    {
        Save();
        Clear();
    }

    private void Clear()
    {
        foreach (var item in _activeHolder.GetChildren())
        {
            Destroy(item.gameObject);
        }
        foreach (var item in _inactiveHolder.GetChildren())
        {
            Destroy(item.gameObject);
        }
        _spawnedSupplies = new List<Supply>();
        _shownSupply = null;
    }

    private void Hide()
    {
        if (_shownSupply != null)
        {
            _shownSupply.transform.DOKill();
            _shownSupply.transform.SetParent(_inactiveHolder);
            _shownSupply.transform.DOLocalMoveY(0, 0.3f);
            _shownSupply = null;
        }
        OnSupplyHidden?.Invoke();
    }

    private Supply SpawnNew(SupplyName supplyName)
    {
        var supply = SuppliesSpawner.instance.Spawn(supplyName);
        supply.OnEmpty += OnSupplyEmpty;
        supply.OnRemainingChanged += SupplyRemainingChanged;
        supply.gameObject.transform.position = _inactiveHolder.transform.position;
        supply.OnSpawned(DataHolder.CurrentData.CurrentSupplies[supply.SupplyName]);
        _spawnedSupplies.Add(supply);
        return supply;
    }

    public void Show(SupplyName supplyName)
    {
        if (_shownSupply == null || supplyName != _shownSupply.SupplyName)
        {
            Debug.Log("Show " + supplyName);
            instance.Hide();

            var supplyData = DataHolder.CurrentData.CurrentSupplies[supplyName];

            if (supplyData.quantity > 0)
            {
                var supply = instance._spawnedSupplies.FirstOrDefault(s => s.SupplyName == supplyName);
                if (supply == null)
                {
                    supply = SpawnNew(supplyName);
                }

                instance._shownSupply = supply;
                supply.gameObject.transform.SetParent(instance._activeHolder);
                supply.gameObject.transform.localPosition = Vector3.zero;

                _activeHolder.transform.DOKill();
                _activeHolder.localPosition = _activeHolderPosition + new Vector3(0, 3, 0);
                _activeHolder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-5, 5)));
                var seq = DOTween.Sequence();
                seq.Append(_activeHolder.transform.DOLocalMove(_activeHolderPosition, 0.5f));
                seq.AppendCallback(delegate { _activeHolder.transform.localPosition = _activeHolderPosition; });
                OnSupplyShown?.Invoke(supplyName);
            }
            else
            {
                Debug.Log("Out of supply " + supplyName);
            }
        }
        else
        {
            Hide();
        }
    }

    private void Save()
    {
        foreach (var supply in _spawnedSupplies)
        {
            var supplyData = supply.GetSupplyData();
            DataHolder.CurrentData.CurrentSupplies[supply.SupplyName] = supplyData;
        }
    }

    private void OnSupplyEmpty(SupplyName supplyName)
    {
        var saveData = DataHolder.CurrentData.CurrentSupplies[supplyName];
        var supply = instance._spawnedSupplies.FirstOrDefault(s => s.SupplyName == supplyName);

        saveData.quantity--;
        saveData.remaining = 1;
        saveData.MeshData = null;

        _spawnedSupplies.Remove(supply);
        _shownSupply = null;
        Destroy(supply.gameObject);
        OnSupplyQuantityChanged?.Invoke(supplyName, saveData.quantity);
        if (saveData.quantity == 0)
        {
            OnOutOfSupply?.Invoke(supplyName);
            DataHolder.CurrentData.CurrentSupplies.Remove(supplyName);
        }
        else
        {
            var seq = DOTween.Sequence();
            seq.AppendInterval(1f);
            seq.AppendCallback(delegate { Show(supplyName); });
            seq.Play();
        }
    }

    private void SupplyRemainingChanged(SupplyName supplyName, float remianing)
    {
        OnSupplyRemainingChanged.Invoke(supplyName, remianing);
        DataHolder.CurrentData.CurrentSupplies[supplyName].remaining = remianing;
    }

    public void OnGameStateChanged(GameState gameState)
    {
        if (gameState != GameState.Cooking)
        {
            Hide();
        }
    }
}

[Serializable]
public class SupplyData
{
    public float remaining = 1;
    public int quantity;
    public int price;
    public MeshData MeshData;

    public SupplyData()
    {
        remaining = 1;
        quantity = 1;
        price = 0;
    }

    public SupplyData(float remaining, int quantity, int price, MeshData meshData)
    {
        this.remaining = remaining;
        this.quantity = quantity;
        this.price = price;
        MeshData = meshData;
    }
}