using System;
using Data;
using Data.Enums;
using Infrastructure.Services.Interfaces;
using Infrastructure.Signals;
using Zenject;

namespace Infrastructure.Managers
{
    public class AudioManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IAudioService _audioService;
        private readonly AudioSettingsModel _audioSettingsModel;

        private float _lastMusicVolume = 1.0f;
        private float _lastSfxVolume = 1.0f;

        public AudioManager(
            SignalBus signalBus,
            IAudioService audioService,
            AudioSettingsModel settingsModel)
        {
            _signalBus = signalBus;
            _audioService = audioService;
            _audioSettingsModel = settingsModel;

            _lastMusicVolume = _audioSettingsModel.MusicVolume > 0 ? _audioSettingsModel.MusicVolume : 1.0f;
            _lastSfxVolume = _audioSettingsModel.SfxVolume > 0 ? _audioSettingsModel.SfxVolume : 1.0f;
        }

        public void Initialize()
        {
            //? Gameplay Logic
            _signalBus.Subscribe<ButtonClick>(OnButtonClick);
            _signalBus.Subscribe<StartGameEvent>(OnStartGame);
            _signalBus.Subscribe<TaskStationStateChangedSignal>(OnTaskStationStateChanged);
            _signalBus.Subscribe<FactoryStateChangedSignal>(OnFactoryStateChanged);
            _signalBus.Subscribe<ProcessedMaterialProducedSignal>(OnProcessedMaterialProduced);
            _signalBus.Subscribe<ComponentProducedSignal>(OnComponentProduced);
            _signalBus.Subscribe<PCAssembledSignal>(OnPCAssembled);
            _signalBus.Subscribe<OrderCompletedSignal>(OnOrderCompleted);

            //? Audio Settings (Options panel)
            _signalBus.Subscribe<SetMusicVolumeEvent>(OnSetMusicVolume);
            _signalBus.Subscribe<SetSfxVolumeEvent>(OnSetSfxVolume);
            _signalBus.Subscribe<SetMusicMuteEvent>(OnSetMusicMuteEvent);
            _signalBus.Subscribe<SetSfxMuteEvent>(OnSetSfxMuteEvent);

            //? Start default music
            _audioService.PlayMusic(MusicType.MainMenu);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ButtonClick>(OnButtonClick);
            _signalBus.Unsubscribe<StartGameEvent>(OnStartGame);
            _signalBus.Unsubscribe<TaskStationStateChangedSignal>(OnTaskStationStateChanged);
            _signalBus.Unsubscribe<FactoryStateChangedSignal>(OnFactoryStateChanged);
            _signalBus.Unsubscribe<ProcessedMaterialProducedSignal>(OnProcessedMaterialProduced);
            _signalBus.Unsubscribe<ComponentProducedSignal>(OnComponentProduced);
            _signalBus.Unsubscribe<PCAssembledSignal>(OnPCAssembled);
            _signalBus.Unsubscribe<OrderCompletedSignal>(OnOrderCompleted);

            _signalBus.Unsubscribe<SetMusicVolumeEvent>(OnSetMusicVolume);
            _signalBus.Unsubscribe<SetSfxVolumeEvent>(OnSetSfxVolume);
            _signalBus.Unsubscribe<SetMusicMuteEvent>(OnSetMusicMuteEvent);
            _signalBus.Unsubscribe<SetSfxMuteEvent>(OnSetSfxMuteEvent);
        }

        private void OnButtonClick(ButtonClick evt)
        {
            _audioService.PlaySfx(SfxType.ButtonClick);
        }

        private void OnStartGame(StartGameEvent evt)
        {
            _audioService.PlayMusic(MusicType.Gameplay);
        }

        private void OnTaskStationStateChanged(TaskStationStateChangedSignal evt)
        {
            if (evt.IsWorking)
            {
                _audioService.PlaySfx(SfxType.TaskStationMining);
            }
            else
            {
                _audioService.StopSfx();
            }
        }

        private void OnFactoryStateChanged(FactoryStateChangedSignal evt)
        {
            if (evt.State == FactoryState.Processing)
            {
                _audioService.PlaySfx(SfxType.FactoryWorking);
            }
        }

        private void OnProcessedMaterialProduced(ProcessedMaterialProducedSignal evt)
        {
            _audioService.PlaySfx(SfxType.ProcessedMaterialProduced);
        }

        private void OnComponentProduced(ComponentProducedSignal evt)
        {
            _audioService.PlaySfx(SfxType.ComponentProduced);
        }

        private void OnPCAssembled(PCAssembledSignal evt)
        {
            _audioService.PlaySfx(SfxType.PCAssembled);
        }

        private void OnOrderCompleted(OrderCompletedSignal evt)
        {
            _audioService.PlaySfx(SfxType.OrderCompleted);
        }

        //? Audio Settings (Options panel)
        private void OnSetMusicVolume(SetMusicVolumeEvent evt)
        {
            if (evt.Volume > 0)
            {
                _lastMusicVolume = evt.Volume;
            }

            _audioService.SetMusicVolume(evt.Volume);
            _audioSettingsModel.MusicVolume = evt.Volume;

            if (evt.Volume > 0 && _audioSettingsModel.IsMusicMuted)
            {
                _signalBus.Fire(new SetMusicMuteEvent(false));
            }
            else if (evt.Volume <= 0 && !_audioSettingsModel.IsMusicMuted)
            {
                _signalBus.Fire(new SetMusicMuteEvent(true));
            }
        }

        private void OnSetSfxVolume(SetSfxVolumeEvent evt)
        {
            if (evt.Volume > 0)
            {
                _lastSfxVolume = evt.Volume;
            }

            _audioService.SetSfxVolume(evt.Volume);
            _audioSettingsModel.SfxVolume = evt.Volume;

            if (evt.Volume > 0 && _audioSettingsModel.IsSfxMuted)
            {
                _signalBus.Fire(new SetSfxMuteEvent(false));
            }
            else if (evt.Volume <= 0 && !_audioSettingsModel.IsSfxMuted)
            {
                _signalBus.Fire(new SetSfxMuteEvent(true));
            }
        }

        private void OnSetMusicMuteEvent(SetMusicMuteEvent evt)
        {
            _audioService.SetMusicMute(evt.IsMusicMuted);
            _audioSettingsModel.IsMusicMuted = evt.IsMusicMuted;

            if (evt.IsMusicMuted)
            {
                _signalBus.Fire(new SetMusicVolumeEvent(0));
            }
            else
            {
                _signalBus.Fire(new SetMusicVolumeEvent(_lastMusicVolume));
            }
        }

        private void OnSetSfxMuteEvent(SetSfxMuteEvent evt)
        {
            _audioService.SetSfxMute(evt.IsSfxMuted);
            _audioSettingsModel.IsSfxMuted = evt.IsSfxMuted;

            if (evt.IsSfxMuted)
            {
                _signalBus.Fire(new SetSfxVolumeEvent(0));
            }
            else
            {
                _signalBus.Fire(new SetSfxVolumeEvent(_lastSfxVolume));
            }
        }
    }
}