using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public partial class DataHolder
{
    [Serializable]
    public class SaveData
    {
        public DateTime LastPlayedDate { get; set; }
        public List<ComboName> OpenedCombos { get; private set; }
        public List<SupplyName> UnlockedSupplies { get; private set; }
        public SerializableDictionary<SupplyName, SupplyData> CurrentSupplies { get; private set; }

        public TileData[] MapData { get; private set; }
        public SerializableDictionary<float2, MapSiteData> SitesData { get; private set; }
        public SerializableDictionary<string, PresetOrderData> PresetOrders { get; private set; }
        public TutorialData TutorialData;

        private int _money;
        private int _dishesCooked;
        private bool _cheatMode;
        private float2 _playerPosition;
        private float2 _currentTown;

        public const int MapSize = 64;
        public bool IsCurrentData => DataHolder.CurrentData == this;

        public int Money
        {
            get => _money;
            set
            {
                Debug.Log("Money changed from " + _money + " to " + value);
                _money = value;
                if (IsCurrentData)
                {
                    OnMoneyChanged?.Invoke(_money);
                }
            }
        }

        public int DishesCooked
        {
            get => _dishesCooked; set
            {
                _dishesCooked = value;
            }
        }

        public bool CheatMode { get => _cheatMode; set => _cheatMode = value; }
        public float2 PlayerPosition { get => _playerPosition; }
        public float2 CurrentTown { get => _currentTown; }

        private event Action i;

        public SaveData(SerializableDictionary<float2, MapSiteData> sites)
        {
            OpenedCombos = new List<ComboName>();
            UnlockedSupplies = new List<SupplyName>();
            CurrentSupplies = new SerializableDictionary<SupplyName, SupplyData>();
            LastPlayedDate = DateTime.Now;
            MapData = new TileData[MapSize * MapSize];
            SitesData = sites;
            PresetOrders = new SerializableDictionary<string, PresetOrderData>();
            for (int i = 0; i < MapData.Length; i++)
            {
                MapData[i] = new TileData(i);
            }
        }

        public void UnlockSupply(SupplyName supplyName)
        {
            if (!UnlockedSupplies.Contains(supplyName))
            {
                UnlockedSupplies.Add(supplyName);
                if (IsCurrentData)
                {
                    OnSupplyUnlocked?.Invoke(supplyName);
                }
            }
        }

        public void SetSupply(SupplyName supplyName, SupplyData supplyData)
        {
            if (!CurrentSupplies.Contains(supplyName))
            {
                CurrentSupplies.Add(supplyName, supplyData);
            }
            else
            {
                CurrentSupplies[supplyName] = supplyData;
            }
            if (IsCurrentData)
            {
                OnSupplyChanged?.Invoke(supplyName, supplyData);
            }
        }

        public void RemoveSupply(SupplyName supplyName, int count)
        {
            if (CurrentSupplies.Contains(supplyName))
            {
                var data = CurrentSupplies[supplyName];
                data.quantity -= count;

                if (IsCurrentData)
                {
                    OnSupplyChanged?.Invoke(supplyName, data);
                    OnSupplyRemoved?.Invoke(supplyName, count);
                }
            }
        }

        public void AddSupply(SupplyName supplyName, int count)
        {
            if (CurrentSupplies.Contains(supplyName))
            {
                var data = CurrentSupplies[supplyName];
                data.quantity += count;
            }
            else
            {
                CurrentSupplies.Add(supplyName, new SupplyData(1, count, 0, null));
            }
            if (IsCurrentData)
            {
                OnSupplyAdded?.Invoke(supplyName, count);
                OnSupplyChanged?.Invoke(supplyName, CurrentSupplies[supplyName]);
            }
        }

        public TileData GetTileData(Vector2Int cell)
        {
            var index = GetFlatIndex(cell);
            return MapData[index];
        }

        public void SetTileData(TileData tileData, Vector2Int cell)
        {
            var index = GetFlatIndex(cell);
            MapData[index] = tileData;
        }

        private int GetFlatIndex(Vector2Int cell)
        {
            var index = ((cell.y + 32) * MapSize + cell.x + 32);
            if (index < 0 || index >= MapSize * MapSize)
            {
                throw new Exception("Cell out of range" + cell);
            }
            return index;
        }

        public void SetPlayerPosition(Vector2 position)
        {
            _playerPosition = position;
        }

        public void SetCurrentTown(Vector2 position)
        {
            _currentTown = position;
        }

        public void AddPresetOrderData(string name, PresetOrderData presetOrderData)
        {
            PresetOrders.Add(name, presetOrderData);
        }
    }

    [Serializable]
    public struct TutorialData
    {
        public bool moveTutorialComplete;
        public bool townTutorialComplete;
        public bool orderTutorialComplete;
    }
}