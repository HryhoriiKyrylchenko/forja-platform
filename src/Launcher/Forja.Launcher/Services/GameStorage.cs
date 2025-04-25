namespace Forja.Launcher.Services;

public class GameStorage
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "games.json");

    public static async Task SaveAsync(IEnumerable<Game> games)
    {
        var json = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(FilePath, json);
    }

    public static async Task<List<Game>> LoadAsync()
    {
        if (!File.Exists(FilePath))
            return new List<Game>();

        var json = await File.ReadAllTextAsync(FilePath);
        return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
    }
}