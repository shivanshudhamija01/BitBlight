using System;
using System.Collections.Generic;
using Data.Enums;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Bitblight/AudioConfigSO")]
    public class AudioConfigSO : ScriptableObject
    {
        [Serializable]
        public struct SfxMapping
        {
            public SfxType type;
            public AudioClip clip;
        }

        [Serializable]
        public struct MusicMapping
        {
            public MusicType type;
            public AudioClip clip;
        }

        [Header("Mappings")]
        public List<SfxMapping> sfxMappings;

        public List<MusicMapping> musicMappings;
    }
}