namespace Forja.Launcher.Services;

public class ApiService
{
    private static readonly HttpClient HttpClient;
    private readonly string _apiUrl = "https://localhost:7052";
    
    static ApiService()
    {
        HttpClient = new HttpClient(new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()  
        });
    }
    
    public async Task<bool> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginUserRequest
        {
            Email = email,
            Password = password
        };

        var response = await HttpClient.PostAsJsonAsync($"{_apiUrl}/Auth/login", loginRequest);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RefreshTokenAsync()
    {
        var response = await HttpClient.PostAsync($"{_apiUrl}/Auth/refresh", null);

        return response.IsSuccessStatusCode;
    }
    
    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        throw new NotImplementedException();
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
}