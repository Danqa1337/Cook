using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class SaveDataIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lastPlayedDateText;
    [SerializeField] private TextMeshProUGUI _dishesCookedText;
    [SerializeField] private TextMeshProUGUI _moneyText;

    [SerializeField] private Button _deleteButton;
    [SerializeField] private GameObject _activePanel;
    private DataHolder.SaveData _saveData;
    private int _index;

    public void DrawSaveData(DataHolder.SaveData saveData, int index)
    {
        _index = index;
        _saveData = saveData;
        if (saveData != null)
        {
            _activePanel.SetActive(true);
            _lastPlayedDateText.text = saveData.LastPlayedDate.ToShortDateString();
            _dishesCookedText.text = "Dishes cooked: " + saveData.DishesCooked;
            _moneyText.text = "Money: " + saveData.Money + "$";
        }
        else
        {
            _activePanel.SetActive(false);
        }
    }

    public void Play()
    {
        PlayerPrefs.SetInt("currentSaveIndex", _index);
        DataHolder.instance.Load();
        Kitchen.instance.StartNewDish();
    }

    public void PlayCheatMode()
    {
        DataHolder.instance.GiveAllSupplies();
        Play();
    }

    public void Delete()
    {
        if (_saveData != null)
        {
            DataLoader.DeleteSave(_index);
            DrawSaveData(null, _index);
        }
    }
}