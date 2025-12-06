
namespace snake_console.Data;

public class MatchRecord
{
    public string Character { get; set; }
    public int Kills { get; set; }
    public string TimeEnds { get; set; }
    public int Score { get; set; }
    public string DateTime { get; set; }
}

public class SettingsData
{
    public int LobbyBgmVolume { get; set; }
    public int GameBgmVolume { get; set; }
    public int LobbySoundEffectVolume { get; set; }
    public int GameSoundEffectVolume { get; set; }
}

public class PlayerData
{
    public string Name { get; set; }
    public int Money { get; set; }
    public List<MatchRecord> History { get; set; } = new List<MatchRecord>();
    public List<string> Characters { get; set; } = new List<string>();
    public SettingsData Settings  { get; set; }
}