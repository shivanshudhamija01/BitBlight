using System;
using System.IO;
using Features.Resources.Interfaces;
using Infrastructure.Services.Interfaces;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Features.Resources
{
    public class ResourceSaveService : IDataPersistence, IInitializable, IDisposable
    {
        private readonly IResourcePersistence _resourcePersistence;
        private string _savePath;

        public ResourceSaveService(IResourcePersistence resourcePersistence)
        {
            _resourcePersistence = resourcePersistence;
        }

        public void Initialize()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "SaveData/inventory.json");
            Debug.Log($"Save path: {_savePath}");
            Load();
            Application.quitting += Save;
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            Application.quitting -= Save;
            Application.focusChanged -= OnFocusChanged;
            Save();
        }

        private void OnFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
                Save();
        }

        public void Save()
        {
            try
            {
                var data = _resourcePersistence.GetSaveData();
                string json = JsonUtility.ToJson(data);
                Debug.Log($"Saving JSON: {json}");

                string directory = Path.GetDirectoryName(_savePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(_savePath, json);
                Debug.Log($"File exists after save: {File.Exists(_savePath)}");
                Debug.Log($"File size: {new FileInfo(_savePath).Length} bytes");
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e}");
            }
        }

        public void Load()
        {
            if (!File.Exists(_savePath)) return;

            try
            {
                string json = File.ReadAllText(_savePath);
                var data = JsonUtility.FromJson<InventorySaveData>(json);
                _resourcePersistence.LoadData(data);
                Debug.Log($"Loaded from: {_savePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed: {e}");
            }
        }
    }
}