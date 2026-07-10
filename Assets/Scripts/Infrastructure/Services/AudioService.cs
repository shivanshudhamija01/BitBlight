using System.Collections.Generic;
using Data.Enums;
using Data.ScriptableObjects;
using Infrastructure.Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services
{
    public class AudioService : IAudioService, IInitializable
    {
        private readonly AudioConfigSO _audioConfig;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;


        private readonly Dictionary<SfxType, AudioClip> _sfxCache = new();
        private readonly Dictionary<MusicType, AudioClip> _musicCache = new();

        public bool IsMusicMuted { get; private set; } = false;
        public bool IsSfxMuted { get; private set; } = false;
        public float SfxVolume { get; private set; } = 1.0f;
        public float MusicVolume { get; private set; } = 1.0f;

        public AudioService(
            AudioConfigSO config,
            [Inject(Id = ServiceKeys.Music)] AudioSource musicSource,
            [Inject(Id = ServiceKeys.Sfx)] AudioSource sfxSource
        )
        {
            _audioConfig = config;
            _musicSource = musicSource;
            _sfxSource = sfxSource;
        }

        public void Initialize()
        {
            InitializeLookups();
        }


        private void InitializeLookups()
        {
            foreach (var item in _audioConfig.sfxMappings)
            {
                _sfxCache[item.type] = item.clip;
            }

            foreach (var item in _audioConfig.musicMappings)
            {
                _musicCache[item.type] = (item.clip);
            }
        }

        public void PlaySfx(SfxType sfxType)
        {
            if (IsSfxMuted) return;
            if (_sfxCache.TryGetValue(sfxType, out var clip))
            {
                _sfxSource.PlayOneShot(clip, SfxVolume);
            }
        }

        public void PlayMusic(MusicType type)
        {
            if (_musicCache.TryGetValue(type, out var clip))
            {
                if (_musicSource.clip == clip && _musicSource.isPlaying) return;

                _musicSource.clip = clip;
                _musicSource.loop = true;
                UpdateMusicOutput();
                _musicSource.Play();
            }
        }

        public void StopSfx()
        {
            _sfxSource.Stop();
        }

        //? Volume Control

        public void SetMusicMute(bool isMusicMuted)
        {
            IsMusicMuted = isMusicMuted;
            UpdateMusicOutput();
        }

        public void SetSfxMute(bool isSfxMuted)
        {
            IsSfxMuted = isSfxMuted;
            UpdateSfxOutput();
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = Mathf.Clamp01(volume);
            UpdateSfxOutput();
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            UpdateMusicOutput();
        }

        private void UpdateMusicOutput()
        {
            _musicSource.volume = IsMusicMuted ? 0 : MusicVolume;
            _musicSource.mute = IsMusicMuted;
        }

        private void UpdateSfxOutput()
        {
            _sfxSource.volume = IsSfxMuted ? 0 : SfxVolume;
            _sfxSource.mute = IsSfxMuted;
        }
    }
}