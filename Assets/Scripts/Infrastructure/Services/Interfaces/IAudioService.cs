using Data.Enums;
using UnityEngine;

namespace Infrastructure.Services.Interfaces
{
    public interface IAudioService
    {
        bool IsMusicMuted { get; }
        bool IsSfxMuted { get; }
        float SfxVolume { get; }
        float MusicVolume { get; }
        void PlaySfx(SfxType type);
        void PlayMusic(MusicType type);
        void StopSfx();
        public void SetMusicMute(bool isMusicMuted);
        public void SetSfxMute(bool isSfxMuted);
        void SetSfxVolume(float volume);
        public void SetMusicVolume(float volume);
    }
}