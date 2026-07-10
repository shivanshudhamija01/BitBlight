using System;
using FairyGUI;
using UI.Views.Abstract;

namespace UI.Views
{
    public class MainMenuView : UIViewBase
    {
        private GComponent _menuPanel;
        private GButton _playButton;
        private GButton _optionsButton;
        private GButton _quitButton;

        public override void CreateUI()
        {
            UIPackage.AddPackage("MainMenu");
            _menuPanel = UIPackage.CreateObject("MainMenu", "MainMenuPanel").asCom;
            GRoot.inst.AddChild(_menuPanel);
            _menuPanel.SetSize(GRoot.inst.width, GRoot.inst.height);
            _menuPanel.AddRelation(GRoot.inst, RelationType.Size);

            _playButton = _menuPanel.GetChild("PlayBtn").asButton;
            _optionsButton = _menuPanel.GetChild("OptionsBtn").asButton;
            _quitButton = _menuPanel.GetChild("QuitBtn").asButton;

            // Hide();
        }

        public override void Show()
        {
            if (_menuPanel != null)
            {
                _menuPanel.visible = true;
            }
        }

        public override void Hide()
        {
            if (_menuPanel != null)
            {
                _menuPanel.visible = false;
            }
        }

        public void OnPlay(Action action) => _playButton.onClick.Add(() => action());
        public void OnOptions(Action action) => _optionsButton.onClick.Add(() => action());
        public void OnQuit(Action action) => _quitButton.onClick.Add(() => action());
    }
}