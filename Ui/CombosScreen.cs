using System;
using UnityEngine;

public class CombosScreen : UiCanvas
{
    [SerializeField] private Transform _holder;
    [SerializeField] private ComboIcon _comboIconPrefab;

    public static event Action<ComboName> OnIconClicked;

    public override void Show()
    {
        base.Show();
        OnOpen();
    }

    public void OnOpen()
    {
        Clear();
        var allCombos = CombosDatabase.GetAllCombos();
        foreach (var item in allCombos)
        {
            SpawnIcon(item.ComboName, DataHolder.CurrentData.OpenedCombos.Contains(item.ComboName));
        }
        for (int i = 0; i < 20 - allCombos.Length; i++)
        {
            SpawnIcon(ComboName.Null, false);
        }
    }

    private ComboIcon SpawnIcon(ComboName comboName, bool opened)
    {
        var icon = Instantiate(_comboIconPrefab.gameObject, _holder).GetComponent<ComboIcon>();
        icon.DrawCombo(comboName, opened);
        icon.OnClick += OnIconClicked;
        return icon;
    }

    private void Clear()
    {
        foreach (var child in _holder.GetChildren())
        {
            Destroy(child.gameObject);
        }
    }

    public void Back()
    {
        GameStateManager.ChangeGameState(GameState.Cooking);
        Timer.instance.Continue();
    }
}