using System;
using Data;
using Data.Enums;
using Infrastructure.Signals;
using UI.Views;
using Zenject;

namespace UI.Presenters
{
    public class OptionsPresenter : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly OptionsView _optionsView;
        private readonly AudioSettingsModel _audioSettingsModel;

        public OptionsPresenter(
            SignalBus signalBus,
            OptionsView optionsView,
            AudioSettingsModel audioSettingsModel)
        {
            _signalBus = signalBus;
            _optionsView = optionsView;
            _audioSettingsModel = audioSettingsModel;
        }

        public void Initialize()
        {
            _optionsView.CreateUI();

            _optionsView.Setup(
                _audioSettingsModel.IsMusicMuted,
                _audioSettingsModel.IsSfxMuted,
                _audioSettingsModel.SfxVolume,
                _audioSettingsModel.MusicVolume
            );
            _optionsView.OnBack(() =>
            {
                _signalBus.Fire(new ButtonClick());
                _signalBus.Fire(new CloseMenuEvent());
            });
            _optionsView.OnMusicMuteBtnChanged(mute =>
            {
                _signalBus.Fire(new ButtonClick());
                _signalBus.Fire(new SetMusicMuteEvent(mute));
            });
            _optionsView.OnSfxMuteBtnChanged(mute =>
            {
                _signalBus.Fire(new ButtonClick());
                _signalBus.Fire(new SetSfxMuteEvent(mute));
            });
            _optionsView.OnMusicSliderChanged(vol => _signalBus.Fire(new SetMusicVolumeEvent(vol)));
            _optionsView.OnSfxSliderChanged(vol => _signalBus.Fire(new SetSfxVolumeEvent(vol)));

            _signalBus.Subscribe<SetMusicVolumeEvent>(OnMusicVolumeChanged);
            _signalBus.Subscribe<SetSfxVolumeEvent>(OnSfxVolumeChanged);
            _signalBus.Subscribe<SetMusicMuteEvent>(OnMusicMuteChanged);
            _signalBus.Subscribe<SetSfxMuteEvent>(OnSfxMuteChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<SetMusicVolumeEvent>(OnMusicVolumeChanged);
            _signalBus.Unsubscribe<SetSfxVolumeEvent>(OnSfxVolumeChanged);
            _signalBus.Unsubscribe<SetMusicMuteEvent>(OnMusicMuteChanged);
            _signalBus.Unsubscribe<SetSfxMuteEvent>(OnSfxMuteChanged);
        }

        private void OnMusicVolumeChanged(SetMusicVolumeEvent evt) => _optionsView.UpdateMusicVolume(evt.Volume);
        private void OnSfxVolumeChanged(SetSfxVolumeEvent evt) => _optionsView.UpdateSfxVolume(evt.Volume);
        private void OnMusicMuteChanged(SetMusicMuteEvent evt) => _optionsView.UpdateMusicMute(evt.IsMusicMuted);
        private void OnSfxMuteChanged(SetSfxMuteEvent evt) => _optionsView.UpdateSfxMute(evt.IsSfxMuted);
    }
}