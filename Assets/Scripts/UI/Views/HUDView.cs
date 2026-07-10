using System;
using FairyGUI;
using UI.Views.Abstract;
using UnityEngine;

namespace UI.Views
{
    public class HUDView : UIViewBase
    {
        private GComponent _inventoryPanel;
        private GButton _pauseButton;
        private GButton _recipesButton;
        private GButton _orderPopupButton;

        private Transition _showTransition;
        private Transition _hideTransition;

        public override void CreateUI()
        {
            var scaler = Stage.inst.gameObject.GetComponent<UIContentScaler>();
            scaler.scaleMode = UIContentScaler.ScaleMode.ScaleWithScreenSize;
            scaler.designResolutionX = 1920;
            scaler.designResolutionY = 1080;
            scaler.screenMatchMode = UIContentScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.ApplyChange();
            GRoot.inst.ApplyContentScaleFactor();
            GRoot.inst.MakeFullScreen();

            UIPackage.AddPackage("HUD");
            _inventoryPanel = UIPackage.CreateObject("HUD", "InventoryHUD").asCom;
            GRoot.inst.AddChild(_inventoryPanel);
            _inventoryPanel.SetSize(GRoot.inst.width, GRoot.inst.height);
            _inventoryPanel.AddRelation(GRoot.inst, RelationType.Size);

            _pauseButton = _inventoryPanel.GetChild("PauseBtn").asButton;
            _recipesButton = _inventoryPanel.GetChild("RecipesBtn").asButton;
            _orderPopupButton = _inventoryPanel.GetChild("OrderPopupBtn").asButton;

            _showTransition = _inventoryPanel.GetTransition("Show");
            _hideTransition = _inventoryPanel.GetTransition("Hide");

            InitialHide();

            Debug.Log($"GRoot Size: {GRoot.inst.width} x {GRoot.inst.height}");
            Debug.Log($"InventoryPanel  Size: {_inventoryPanel.width} x {_inventoryPanel.height}");
        }

        private void InitialHide()
        {
            if (_inventoryPanel != null)
            {
                _inventoryPanel.visible = false;
            }
        }

        public override void Show()
        {
            if (_inventoryPanel != null)
            {
                _hideTransition.Stop();
                _inventoryPanel.visible = true;
                _showTransition.Play();
            }
        }

        public override void Hide()
        {
            if (_inventoryPanel != null && _inventoryPanel.visible)
            {
                _showTransition.Stop();
                _hideTransition.Play((() => _inventoryPanel.visible = false));
            }
        }

        public void OnPause(Action action) => _pauseButton.onClick.Add((() => action()));
        public void OnRecipes(Action action) => _recipesButton.onClick.Add(() => action());
        public void OnOrderPopup(Action action) => _orderPopupButton.onClick.Add(() => action());

        public GList GetResourceList()
        {
            return _inventoryPanel.GetChild("ResourceList").asList;
        }

        public GLoader GetMiniMapLoader()
        {
            return _inventoryPanel.GetChild("MinimapLoader")?.asLoader;
        }
    }
}