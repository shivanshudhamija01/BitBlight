using System;
using System.Collections.Generic;
using System.Linq;
using Data.ScriptableObjects.Resources;
using Features.Resources;
using Zenject;
using FairyGUI;
using Features.Resources.Interfaces;
using Infrastructure.Signals;
using UI.Views;
using UnityEngine;

namespace UI.Presenters
{
    public class HUDPresenter : IInitializable, IDisposable
    {
        private readonly IResourcePersistence _resourcePersistence;
        private readonly SignalBus _signalBus;
        private readonly HUDView _hudView;

        private readonly RawMaterialData[] _rawMaterials;
        private readonly ProcessedMaterialData[] _processedMaterials;

        private GList _list;
        private List<ResourceEntry> _cachedData;

        public HUDPresenter(
            IResourcePersistence resourcePersistence,
            SignalBus signalBus,
            HUDView hudView,
            RawMaterialData[] rawMaterials,
            ProcessedMaterialData[] processedMaterials
        )
        {
            _resourcePersistence = resourcePersistence;
            _signalBus = signalBus;
            _hudView = hudView;
            _rawMaterials = rawMaterials;
            _processedMaterials = processedMaterials;
        }

        public void Initialize()
        {
            _hudView.CreateUI();

            _hudView.OnPause((() =>
            {
                Debug.Log("PAUSE");
                _signalBus.Fire(new PauseGameEvent());
            }));

            _hudView.OnRecipes((() =>
            {
                Debug.Log("RECIPES");
                _signalBus.Fire(new ButtonClick());
                _signalBus.Fire(new GoToRecipesPanelEvent());
            }));
            _hudView.OnOrderPopup((() =>
            {
                Debug.Log("ORDER POPUP");
                _signalBus.Fire(new ShowCurrentOrderEvent());
            }));

            _list = _hudView.GetResourceList();

            _list.defaultItem = "ui://HUD/InventoryItem";
            _list.SetVirtual();
            _list.itemRenderer = RenderItem;

            _signalBus.Subscribe<ResourceChangedSignal>(OnResourceChanged);
            RefreshUI();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ResourceChangedSignal>(OnResourceChanged);
        }

        private void OnResourceChanged(ResourceChangedSignal signal)
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            _cachedData = _resourcePersistence.GetSaveData().entries;
            _list.numItems = _cachedData.Count;
            _list.RefreshVirtualList();
        }

        private void RenderItem(int index, GObject obj)
        {
            GComponent item = obj.asCom;
            var data = _cachedData[index];

            // item.GetChild("title").text = data.enumValue;
            item.GetChild("count").text = data.amount.ToString();

            GLoader iconLoader = item.GetChild("icon").asLoader;
            if (iconLoader != null)
            {
                Sprite sprite = GetSprite(data.enumValue);
                iconLoader.SetSize(80, 80);
                iconLoader.fill = FillType.ScaleFree;
                iconLoader.texture = sprite != null ? new NTexture(sprite) : null;
            }
        }

        private Sprite GetSprite(string enumValue)
        {
            var raw = _rawMaterials.FirstOrDefault(m => m.type.ToString() == enumValue);
            if (raw != null) return raw.icon;

            var processed = _processedMaterials.FirstOrDefault(m => m.type.ToString() == enumValue);
            if (processed != null) return processed.icon;

            return null;
        }
    }
}