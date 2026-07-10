using System;
using FairyGUI;
using UI.Views.Abstract;

namespace UI.Views
{
    public class PauseView : UIViewBase
    {
        private GComponent _pauseRoot;
        private GComponent _pausePanel;
        private GButton _resumeButton;
        private GButton _optionsButton;
        private GButton _mainMenuButton;

        private Transition _showTransition;
        private Transition _hideTransition;

        public override void CreateUI()
        {
            UIPackage.AddPackage("Pause");
            _pauseRoot = UIPackage.CreateObject("Pause", "PauseRoot").asCom;
            GRoot.inst.AddChild(_pauseRoot);

            _pauseRoot.SetSize(GRoot.inst.width, GRoot.inst.height);
            _pauseRoot.AddRelation(GRoot.inst, RelationType.Size);

            _pausePanel = _pauseRoot.GetChild("PausePanel").asCom;
            _resumeButton = _pausePanel.GetChild("ResumeBtn").asButton;
            _optionsButton = _pausePanel.GetChild("OptionsBtn").asButton;
            _mainMenuButton = _pausePanel.GetChild("MainMenuBtn").asButton;

            _showTransition = _pauseRoot.GetTransition("Show");
            _hideTransition = _pauseRoot.GetTransition("Hide");

            InitialHide();
        }

        private void InitialHide()
        {
            if (_pauseRoot != null)
            {
                _pauseRoot.visible = false;
            }
        }

        public override void Show()
        {
            if (_pauseRoot != null)
            {
                _hideTransition.Stop();
                _pauseRoot.visible = true;
                _showTransition.Play();
            }
        }

        public override void Hide()
        {
            if (_pauseRoot != null && _pauseRoot.visible)
            {
                _showTransition.Stop();
                _hideTransition.Play(() => _pauseRoot.visible = false);
            }
        }

        public void OnResume(Action action) => _resumeButton.onClick.Add(() => action());
        public void OnOptionsMenu(Action action) => _optionsButton.onClick.Add(() => action());
        public void OnMainMenu(Action action) => _mainMenuButton.onClick.Add(() => action());
    }
}