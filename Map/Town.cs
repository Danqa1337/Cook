using System;
using UnityEngine;
using UnityEngine.UI;

public class Town : MapObjectComponent
{
    public event Action OnPlayerEnterEvent;

    public event Action OnPlayerExitEvent;

    public event Action OnPlayerStayEvent;

    [SerializeField] private PresetOrder[] _presetOrders;

    public static Town Current;

    public PresetOrder[] PresetOrders { get => _presetOrders; }

    public static event Action<Town> OnCurrentTownChanged;

    private void OnEnable()
    {
        DataHolder.OnLoaded += OnLoaded;
    }

    private void OnDisable()
    {
        DataHolder.OnLoaded -= OnLoaded;
    }

    private void OnLoaded(DataHolder.SaveData saveData)
    {
        foreach (var item in _presetOrders)
        {
            if (!saveData.PresetOrders.Contains(item.name))
            {
                saveData.PresetOrders.Add(item.name, new PresetOrderData());
            }
        }
    }

    protected override void OnPlayerEnter()
    {
        Debug.Log("Player entered town");
        Current = this;
        OnCurrentTownChanged?.Invoke(this);
        OnPlayerEnterEvent?.Invoke();
        DataHolder.CurrentData.SetCurrentTown(transform.localPosition);
        DataHolder.instance.Save();
    }

    protected override void OnPlayerExit()
    {
        Debug.Log("Player exited town");
        OnPlayerExitEvent?.Invoke();
    }

    protected override void OnPlayerStay()
    {
        OnPlayerStayEvent?.Invoke();
    }
}