using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FastTravelUi : UiCanvas
{
    [SerializeField] private CameraFollowPoint _travelCameraPoint;
    [SerializeField] private TextMeshProUGUI _costLabel;
    private Vector2[] _towns;
    private int _selectedIndex;

    public override void Show()
    {
        base.Show();
        _towns = DataHolder.CurrentData.SitesData.Where(s => s.Value.MapSiteType == MapSiteType.Town && s.Value.IsVisited).Select(s => s.Key.ToVector2()).ToArray();
        for (int i = 0; i < _towns.Length; i++)
        {
            if (_towns[i] == DataHolder.CurrentData.CurrentTown.ToVector2())
            {
                _selectedIndex = i;
                break;
            }
        }
        ShowSelectedTown();
    }

    public void Next()
    {
        _selectedIndex = (_selectedIndex + 1) % _towns.Length;
        ShowSelectedTown();
    }

    public void Prev()
    {
        _selectedIndex--;
        if (_selectedIndex == -1) _selectedIndex += _towns.Length;

        ShowSelectedTown();
    }

    private void ShowSelectedTown()
    {
        var townPos = Map.LocalToWorld(_towns[_selectedIndex]);
        _travelCameraPoint.transform.position = new Vector3(townPos.x, townPos.y, _travelCameraPoint.transform.position.z);
        _travelCameraPoint.Activate();
        var cost = GetCost();
        _costLabel.text = cost.ToString() + "$";
        if (DataHolder.CurrentData.Money >= cost)
        {
            _costLabel.color = Color.white;
        }
        else
        {
            _costLabel.color = Color.red;
        }
    }

    private int GetCost()
    {
        return (int)(DataHolder.CurrentData.CurrentTown.ToVector2() - _towns[_selectedIndex]).magnitude * 4;
    }

    public void Travel()
    {
        var cost = GetCost();
        if (DataHolder.CurrentData.Money >= cost)
        {
            Player.instance.MoveToPosition(_towns[_selectedIndex]);
            DataHolder.CurrentData.Money -= cost;
            Back();
        }
        else
        {
            _costLabel.transform.DOShakeScale(1);
        }
    }

    public void Back()
    {
        GameStateManager.ChangeGameState(GameState.Cooking);
    }
}