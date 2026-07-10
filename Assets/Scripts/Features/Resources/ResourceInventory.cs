using System;
using System.Collections.Generic;
using Features.Resources.Interfaces;
using Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Features.Resources
{
    public class ResourceInventory : IResourceReader, IResourceWriter, IResourcePersistence
    {
        private readonly Dictionary<Type, Dictionary<Enum, int>> _resources = new();
        private readonly SignalBus _signalBus;

        public ResourceInventory(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        #region Save & Load

        public InventorySaveData GetSaveData()
        {
            var saveData = new InventorySaveData();
            foreach (var category in _resources)
            {
                foreach (var item in category.Value)
                {
                    saveData.entries.Add(new ResourceEntry
                    {
                        typeName = category.Key.AssemblyQualifiedName,
                        enumValue = item.Key.ToString(),
                        amount = item.Value
                    });
                }
            }

            return saveData;
        }

        public void LoadData(InventorySaveData data)
        {
            if (data == null || data.entries == null) return;

            _resources.Clear();

            foreach (var entry in data.entries)
            {
                Type type = Type.GetType(entry.typeName);
                if (type == null) continue;

                try
                {
                    Enum enumVal = (Enum)Enum.Parse(type, entry.enumValue);

                    if (!_resources.TryGetValue(type, out var storage))
                    {
                        storage = new Dictionary<Enum, int>();
                        _resources.Add(type, storage);
                    }

                    storage[enumVal] = entry.amount;
                    _signalBus.Fire(new ResourceChangedSignal(enumVal, entry.amount));
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Inventory] Failed to load {entry.enumValue}: {e.Message}");
                }
            }
        }

        #endregion

        #region Add & Remove

        public void Add<T>(T type, int amount) where T : Enum
        {
            if (amount <= 0) return;

            var enumType = typeof(T);

            if (!_resources.TryGetValue(enumType, out var storage))
            {
                storage = new Dictionary<Enum, int>();
                _resources.Add(enumType, storage);
            }

            storage.TryGetValue(type, out var current);
            current += amount;
            storage[type] = current;

            _signalBus.Fire(new ResourceChangedSignal(type, storage[type]));
        }

        public bool Remove<T>(T type, int amount) where T : Enum
        {
            if (amount <= 0)
                return true;

            var enumType = typeof(T);

            if (!_resources.TryGetValue(enumType, out var storage))
                return false;

            if (!storage.TryGetValue(type, out var current))
                return false;

            if (current < amount)
                return false;

            current -= amount;

            if (current <= 0)
            {
                storage.Remove(type);
                _signalBus.Fire(new ResourceChangedSignal(type, 0));
            }
            else
            {
                storage[type] = current;
                _signalBus.Fire(new ResourceChangedSignal(type, current));
            }

            return true;
        }

        #endregion

        #region Check

        public bool Has<T>(T type, int amount) where T : Enum
        {
            if (amount <= 0) return true;

            var enumType = typeof(T);

            return _resources.TryGetValue(enumType, out var storage)
                   && storage.TryGetValue(type, out var current)
                   && current >= amount;
        }

        public int GetAmount<T>(T type, int amount) where T : Enum
        {
            return _resources.TryGetValue(typeof(T), out var storage)
                   && storage.TryGetValue(type, out var current)
                ? current
                : 0;
        }

        #endregion
    }

    [Serializable]
    public class InventorySaveData
    {
        public List<ResourceEntry> entries = new();
    }

    [Serializable]
    public class ResourceEntry
    {
        public string typeName;
        public string enumValue;
        public int amount;
    }
}