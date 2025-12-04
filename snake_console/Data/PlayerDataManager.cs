using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace snake_console.Data;

public class PlayerDataManager
{

    public PlayerDataManager() {}

    public string GetSaveFolder()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SnakeConsole"
        );
        Directory.CreateDirectory(folder);
        return folder;
    }

    public string GetSavePath() => Path.Combine(GetSaveFolder(), "data.json");
    public string GetHashPath() => Path.Combine(GetSaveFolder(), "data.hash");


    public void Save(PlayerData p)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(p, options);

        string tempPath = GetSavePath() + ".tmp";
        File.WriteAllText(tempPath, json, Encoding.UTF8);

        byte[] hashBytes;
        using (var sha = SHA256.Create())
        {
            hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
        }

        File.WriteAllBytes(GetHashPath(), hashBytes);
        File.Move(tempPath, GetSavePath(), overwrite: true);
    }

    public PlayerData Load()
    {
        string path = GetSavePath();
        string hashPath = GetHashPath();

        if (!File.Exists(path) || !File.Exists(hashPath)) return null;

        string json = File.ReadAllText(path, Encoding.UTF8);

        byte[] savedHash = File.ReadAllBytes(hashPath);

        byte[] currentHash;
        using (var sha = SHA256.Create())
        {
            currentHash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
        }

        if (savedHash.Length != currentHash.Length) return null;

        for (int i = 0; i < savedHash.Length; i++)
        {
            if (savedHash[i] != currentHash[i]) return null;
        }

        return JsonSerializer.Deserialize<PlayerData>(json);
    }

}