namespace snake_console.Utils.Sounds;

public static class GlobalAudio
{
    private static AudioSystem _system;
    private static bool _isPlaying = false;

    private static AudioSystem System
    {
        get
        {
            if (_system == null)
                _system = new AudioSystem();
            return _system;
        }
    }

    public static void PlayBgm(string path, float volume = 0.3f)
    {
        System.PlayMusicLoop(path, volume);
    }

    public static bool IsPlaying => _isPlaying;
    public static void PlayBgmIfNotPlaying(string path, float volume = 0.3f)
    {
        if (!_isPlaying)
        {
            PlayBgm(path, volume);
        }
    }

    public static void PlaySound(string path, float volume = 1f)
    {
        System.PlaySoundEffect(path, volume);
    }

    public static void StopAll()
    {
        _system?.Dispose();
        _system = null;
    }
}
