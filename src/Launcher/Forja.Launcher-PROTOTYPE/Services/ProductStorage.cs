namespace Forja.Launcher.Services;

public class ProductStorage
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "installed_games.json");

    public static async Task SaveInstalledGamesAsync(IEnumerable<InstalledGameModel> games)
    {
        var json = JsonSerializer.Serialize(games, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        try
        {
            await File.WriteAllTextAsync(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    public static async Task<List<InstalledGameModel>> LoadInstalledGamesAsync()
    {
        if (!File.Exists(FilePath))
            return [];

        try
        {
            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<InstalledGameModel>>(json) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}