namespace snake_console.Utils;

using System;
using System.IO;

public class Resources
{

    public static string game_bgm = "game_bgm.mp3";
    public static string lobby_bgm = "lobby_bgm.wav";

    public static string click_sound = "click_sound.wav";
    public static string switch_sound = "switch_sound.wav";
    public static string gameover_time_up_sound = "gameover_times_up_sound.mp3";
    public static string gameover_sound = "gameover_sound.mp3";

    public static string snake_eating_sound = "snake_eating_sound.mp3";
    public static string snake_eating_crash_sound = "snake_eating_crash_sound.mp3";

    public static string getMusic(string fileName)
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Assets", "Sounds", "Musics", fileName);
    }

    public static string getSoundEffect(string fileName)
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Assets", "Sounds", "SoundEffects", fileName);
    }

    public static string getIcon()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Assets", "icon.png");
    }

}