namespace Data
{
    public class AudioSettingsModel
    {
        public float MusicVolume { get; set; } = 1.0f;
        public float SfxVolume { get; set; } = 1.0f;
        public bool IsMusicMuted { get; set; } = false;
        public bool IsSfxMuted { get; set; } = false;
    }
}