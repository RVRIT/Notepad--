using System.IO;
using System.Text.Json;

public static class ConfigService
{
    private static readonly string ConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Notepad", "config.json");

    public static void Save(AppConfig config)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigPath, json);
    }

    public static AppConfig Load()
    {
        if (!File.Exists(ConfigPath)) return new AppConfig();
        try
        {
            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }
        catch { return new AppConfig(); }
    }
}