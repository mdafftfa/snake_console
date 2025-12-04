using System.Collections;

namespace snake_console.Data;

public class MatchRecord
{
    public int Kills { get; set; }
    public string Time { get; set; }
    public int Score { get; set; }
    public DateTime DateTime { get; set; }
}

public class SettingsData
{
    public int MusicVolume { get; set; }
}

public class PlayerData
{
    public string Name { get; set; }
    public int Money { get; set; }
    public List<MatchRecord> History { get; set; }
    public List<string> Snakes { get; set; }
    public SettingsData Settings  { get; set; }
}