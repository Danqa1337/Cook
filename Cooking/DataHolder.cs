using System;
using System.IO;
using UnityEngine;
using static DataHolder;

public partial class DataHolder : Singleton<DataHolder>
{
    [SerializeField] private SaveData _saveData;
    [SerializeField] private string _saveDataPath;
    private string DirectoryPath => Application.persistentDataPath + _saveDataPath;
    private string FullPath => Application.persistentDataPath + _saveDataPath + "SaveData";
    public static SaveData CurrentData { get => instance._saveData; }

    public static event Action<SupplyName, SupplyData> OnSupplyChanged;

    public static event Action<SupplyName, int> OnSupplyAdded;

    public static event Action<SupplyName, int> OnSupplyRemoved;

    public static event Action<int> OnMoneyChanged;

    public static event Action<SupplyName> OnSupplyUnlocked;

    public static event Action<SaveData> OnLoaded;

    [ContextMenu("Load")]
    public void Load()
    {
        var data = DataLoader.Load(GetCurrentSaveIndex());
        if (data != null)
        {
            _saveData = data;
        }
        else
        {
            _saveData = DefaultDataFactory.GetDefaultData();
        }
        OnLoaded?.Invoke(_saveData);
        Debug.Log("Load complete");
    }

    [ContextMenu("Save")]
    public void Save()
    {
        DataLoader.Save(_saveData, GetCurrentSaveIndex());
    }

    [ContextMenu("Reset Saves")]
    public void DeleteSaves()
    {
        DataLoader.Load(GetCurrentSaveIndex());
    }

    [ContextMenu("Give All Supplies")]
    public void GiveAllSupplies()
    {
        _saveData = DefaultDataFactory.GetAllSuppliesData();
        Save();
    }

    private int GetCurrentSaveIndex()
    {
        if (PlayerPrefs.HasKey("currentSaveIndex"))
        {
            return PlayerPrefs.GetInt("currentSaveIndex");
        }
        return 0;
    }
}

public static class DataLoader
{
    private const string _saveDataPath = "Saves/";
    private static string DirectoryPath => Application.persistentDataPath + _saveDataPath;
    private static string FullPath => Application.persistentDataPath + _saveDataPath + "SaveData";

    public static DataHolder.SaveData Load(int index)
    {
        if (File.Exists(FullPath + index))
        {
            DataHolder.SaveData result;
            if (BinarySerializer.TryRead<DataHolder.SaveData>(FullPath + index, out result))
            {
                return result;
            }
            return null;
        }
        else
        {
            Debug.Log("Save " + index + "doens't exist");
            return null;
        }
    }

    public static void Save(DataHolder.SaveData saveData, int index)
    {
        if (saveData == null)
        {
            DeleteSave(index);
        }
        else
        {
            saveData.LastPlayedDate = DateTime.Now;
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            BinarySerializer.Write(saveData, FullPath + index);
        }
        Debug.Log("Save complete: " + index);
    }

    public static void DeleteSave(int index)
    {
        File.Delete(FullPath + index);
    }
}