namespace Forja.Launcher.Services;

public class ApiService
{
    private static readonly HttpClient HttpClient;
    private readonly string _apiUrl = "https://localhost:7052";
    private static readonly string ApiKey = "Forja-Launcher-API-Key";

    
    static ApiService()
    {
        HttpClient = new HttpClient(new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()  
        });
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "Forja-Launcher/1.0");
        HttpClient.DefaultRequestHeaders.Add("X-API-Key", ApiKey);
    }
    
    public async Task<bool> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginUserRequest
        {
            Email = email,
            Password = password
        };

        var response = await HttpClient.PostAsJsonAsync($"{_apiUrl}/api/Auth/login", loginRequest);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RefreshTokenAsync()
    {
        var response = await HttpClient.PostAsync($"{_apiUrl}/api/Auth/refresh", null);

        return response.IsSuccessStatusCode;
    }
    
    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        var response = await HttpClient.GetFromJsonAsync<IEnumerable<Game>>($"{_apiUrl}/api/Library/games");
        return response ?? Enumerable.Empty<Game>();
    }

    public async Task<string> GetLatestVersionAsync(string gameId)
    {
        var response = await HttpClient.GetStringAsync($"{_apiUrl}/games/{gameId}/version");
        return response; 
    }

    public async Task<string> GetDownloadUrlAsync(string gameId, string version)
    {
        return await HttpClient.GetStringAsync($"{_apiUrl}/games/{gameId}/download?version={version}");
    }
    
    public async Task ReportPlayedTimeAsync(string libraryGameId, TimeSpan duration)
    {
        var payload = new PlayedTimeReport
        {
            LibraryGameId = libraryGameId,
            SecondsPlayed = (int)duration.TotalSeconds
        };

        var response = await HttpClient.PostAsJsonAsync($"{_apiUrl}/UserLibrary/game", payload);

        if (!response.IsSuccessStatusCode)
        {
            // Handle error (log, retry, etc.)
        }
    }
    
    public async Task DownloadFileAsync(string url, string destinationPath, IProgress<double>? progress = null)
    {
        using var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
        var canReportProgress = totalBytes != -1 && progress != null;

        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = File.Create(destinationPath);
        var buffer = new byte[81920];
        long totalRead = 0;

        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            totalRead += bytesRead;
            if (canReportProgress)
                progress?.Report((double)totalRead / totalBytes);
        }
    }
}