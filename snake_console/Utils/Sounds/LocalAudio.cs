namespace snake_console.Utils.Sounds;

public static class LocalAudio
{
    private static AudioSystem _system;

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

    public static void PlaySoundEffect(string path, float volume = 1f)
    {
        System.PlaySoundEffect(path, volume);
    }

    public static void StopAll()
    {
        _system?.Dispose();
        _system = null;
    }
}
