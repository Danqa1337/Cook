using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Order
{
    public event Action<Dish> OnCompleted;

    public event Action OnCanceled;

    public event Action OnSubmited;

    public event Action OnDeclined;

    public event Action<AppriceResult> OnAppriced;

    [SerializeField] private int _baseReward = 25;
    [SerializeField] private List<InterStageData> _interStages;
    [SerializeField] private Vector2 _targetPosition;
    [SerializeField] private List<SupplyName> _forbidenSupplies = new List<SupplyName>();
    public Vector2 TargetPosition { get => _targetPosition; set => _targetPosition = value; }
    public List<InterStageData> InterStages { get => _interStages; set => _interStages = value; }
    public List<SupplyName> ForbidenSupplies { get => _forbidenSupplies; }
    public int BaseReward { get => _baseReward; }

    public Order(Vector2 position, int reward, List<InterStageData> interStages, List<SupplyName> forbidenSupplies)
    {
        InterStages = interStages;
        _targetPosition = position;
        _forbidenSupplies = forbidenSupplies;
        _baseReward = reward;
    }

    public Order()
    {
    }

    public override string ToString()
    {
        return "Order :" + TargetPosition;
    }

    public void Complete(Dish dish)
    {
        OnCompleted?.Invoke(dish);
    }

    public void Cancel()
    {
        OnCanceled?.Invoke();
    }

    public void Decline()
    {
        OnDeclined?.Invoke();
    }

    public void Submit()
    {
        OnSubmited?.Invoke();
    }

    public void Apprice(AppriceResult appriceResult)
    {
        OnAppriced?.Invoke(appriceResult);
        DataHolder.CurrentData.Money += appriceResult.FinalValue;
    }

    public void CompleteInterStage(Vector2 position)
    {
        if (InterStages.Any(i => i.position == position))
        {
            Debug.Log("Stage completed");
            InterStages.FirstOrDefault(i => i.position == position).reached = true;
        }
    }
}

[Serializable]
public class InterStageData
{
    public Vector2 position;
    [NonSerialized] public bool reached;

    public InterStageData(Vector2 position)
    {
        this.position = position;
    }
}