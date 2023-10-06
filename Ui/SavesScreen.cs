using UnityEngine;

public class SavesScreen : UiCanvas
{
    [SerializeField] private SaveDataIcon[] _icons;

    public override void Show()
    {
        base.Show();
        for (int i = 0; i < 3; i++)
        {
            var data = DataLoader.Load(i);
            var icon = _icons[i];
            icon.DrawSaveData(data, i);
        }
    }

    public void Back()
    {
        GameStateManager.ChangeGameState(GameState.MainMenu);
    }
}