using System;
using FairyGUI;
using UI.Views.Abstract;

namespace UI.Views
{
    public class OptionsView : UIViewBase
    {
        private GComponent _optionsPanel;
        private GSlider _musicSlider;
        private GSlider _sfxSlider;
        private GButton _musicMuteBtn;
        private GButton _sfxMuteBtn;
        private GButton _backBtn;


        public override void CreateUI()
        {
            UIPackage.AddPackage("Options");
            _optionsPanel = UIPackage.CreateObject("Options", "OptionsPanel").asCom;
            GRoot.inst.AddChild(_optionsPanel);
            _optionsPanel.SetSize(GRoot.inst.width, GRoot.inst.height);
            _optionsPanel.AddRelation(GRoot.inst, RelationType.Size);

            _musicSlider = _optionsPanel
                .GetChild("MusicSlider").asSlider;
            _sfxSlider = _optionsPanel
                .GetChild("SfxSlider").asSlider;
            _musicMuteBtn = _optionsPanel
                .GetChild("MusicMuteBtn").asButton;
            _sfxMuteBtn = _optionsPanel
                .GetChild("SfxMuteBtn").asButton;
            _backBtn = _optionsPanel
                .GetChild("BackBtn").asButton;

            Hide();
        }

        public override void Show()
        {
            if (_optionsPanel != null)
            {
                _optionsPanel
                    .visible = true;
            }
        }

        public override void Hide()
        {
            if (_optionsPanel != null)
            {
                _optionsPanel
                    .visible = false;
            }
        }

        public void SetBackgroundMode(bool visible)
        {
            var bg = _optionsPanel.GetChild("Background");
            if (bg != null) bg.visible = visible;
        }

        //? Initializes UI state from saved audio settings. 
        public void Setup(bool isMusicMuted, bool isSfxMuted, float sfxVolume, float musicVolume)
        {
            _musicMuteBtn.selected = isMusicMuted;
            _sfxMuteBtn.selected = isSfxMuted;
            _sfxSlider.value = sfxVolume * 100f;
            _musicSlider.value = musicVolume * 100f;
        }

        //? Registers callback
        public void OnMusicSliderChanged(Action<float> action) =>
            _musicSlider.onChanged.Add(() => action((float)_musicSlider.value / 100f));

        public void OnSfxSliderChanged(Action<float> action) =>
            _sfxSlider.onChanged.Add(() => action((float)_sfxSlider.value / 100f));

        public void OnMusicMuteBtnChanged(Action<bool> action) =>
            _musicMuteBtn.onChanged.Add((() => action.Invoke(_musicMuteBtn.selected)));

        public void OnSfxMuteBtnChanged(Action<bool> action) =>
            _sfxMuteBtn.onClick.Add(() => action.Invoke(_sfxMuteBtn.selected));

        public void OnBack(Action action) => _backBtn.onClick.Add(() => action());


        //? UI Update Methods For Events
        public void UpdateMusicVolume(float value) => _musicSlider.value = value * 100f;

        public void UpdateSfxVolume(float value) => _sfxSlider.value = value * 100f;

        public void UpdateMusicMute(bool isMusicMuted) => _musicMuteBtn.selected = isMusicMuted;

        public void UpdateSfxMute(bool isSfxMuted) => _sfxMuteBtn.selected = isSfxMuted;
    }
}