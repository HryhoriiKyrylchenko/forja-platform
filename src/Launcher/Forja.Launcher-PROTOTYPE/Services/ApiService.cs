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

    private async Task<bool> RefreshTokenAsync()
    {
        var response = await HttpClient.PostAsync($"{_apiUrl}/api/Auth/refresh", null);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<List<LibraryGameModel>> GetAllGamesAsync()
    {
        var result = await SendWithRefreshAsync<IEnumerable<LibraryGameModel>>(() =>
            HttpClient.GetAsync($"{_apiUrl}/api/UserLibrary/launcher")) ?? [];
        
        return result.ToList();
    }
    
    public async Task DownloadFileInChunksAsync(
        string objectPath,
        string destinationPath,
        int chunkSize = 1 * 1024 * 1024,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = await GetFileMetadataAsync(objectPath, cancellationToken);
        if (fileMetadata is null)
        {
            throw new InvalidOperationException("Unable to determine file metadata.");
        }
        
        if (fileMetadata.Size <= 0)
            throw new InvalidOperationException("Unable to determine file size.");

        var totalChunks = (long)Math.Ceiling((double)fileMetadata.Size / chunkSize);
        long totalBytesDownloaded = 0;

        await using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);

        for (long chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++)
        {
            var offset = chunkIndex * chunkSize;
            var length = Math.Min(chunkSize, fileMetadata.Size - offset);

            var url = $"{_apiUrl}/api/Files/chunk?objectPath={Uri.EscapeDataString(objectPath)}&offset={offset}&length={length}";

            using var response = await SendWithRefreshResponseAsync(() => 
                HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken));
            
            response.EnsureSuccessStatusCode();

            await using var chunkStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            await chunkStream.CopyToAsync(fileStream, cancellationToken);

            totalBytesDownloaded += length;

            progress?.Report((double)totalBytesDownloaded / fileMetadata.Size);
        }
    }
    
    private async Task<StorageMetadata?> GetFileMetadataAsync(string objectPath, CancellationToken cancellationToken = default)
    {
        var url = $"{_apiUrl}/api/Files/metadata?objectPath={Uri.EscapeDataString(objectPath)}";

        var response = await HttpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var metadata = await response.Content.ReadFromJsonAsync<StorageMetadata>(cancellationToken);
        return metadata;
    }
    
    private async Task<T?> SendWithRefreshAsync<T>(Func<Task<HttpResponseMessage>> sendRequest)
    {
        var response = await SendWithRefreshResponseAsync(sendRequest);

        if (!response.IsSuccessStatusCode)
            return default;

        var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private async Task<HttpResponseMessage> SendWithRefreshResponseAsync(Func<Task<HttpResponseMessage>> sendRequest)
    {
        var response = await sendRequest();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshSuccess = await RefreshTokenAsync();

            if (refreshSuccess)
            {
                response = await sendRequest();
            }
        }

        return response;
    }
    
    public async Task<bool> ReportPlayedTimeAsync(Guid libraryGameId, TimeSpan duration)
    {
        var payload = new PlayedTimeReport
        {
            Id = libraryGameId,
            TimePlayed = duration
        };

        var response = await SendWithRefreshResponseAsync(() =>
            HttpClient.PutAsJsonAsync($"{_apiUrl}/api/UserLibrary/game", payload));

        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine("Failed to send played time report.");
            return false;
        }

        return true;
    }
}