namespace Forja.Infrastructure.Keycloak;

public class KeycloakClient : IKeycloakClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _realm;
    private readonly string _clientId;
    private readonly string _clientUUID;
    private readonly string _clientSecret;
    
    public KeycloakClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["Keycloak:BaseUrl"];
        _realm = configuration["Keycloak:Realm"];
        _clientId = configuration["Keycloak:ClientId"];
        _clientUUID = configuration["Keycloak:ClientUUID"];
        _clientSecret = configuration["Keycloak:ClientSecret"];
    }
    
    public async Task<string> CreateUserAsync(KeycloakUser user)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/admin/realms/{_realm}/users");

        var accessToken = await ObtainAdminToken();
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userContent = new
        {
            username = user.Email,
            email = user.Email,
            emailVerified = false,
            enabled = true,
            credentials = new[]
            {
                new
                {
                    temporary = false,
                    type = "password",
                    value = user.Password
                }
            }
        };
        
        var jsonContent = JsonSerializer.Serialize(userContent);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        
        if (response.Headers.Location != null)
        {
            var userUrl = response.Headers.Location.ToString();
            var userId = userUrl.Split('/').Last();
            return userId;
        }
        else
        {
            throw new Exception("User created, but the Location header is missing. Cannot retrieve user ID.");
        }
    }
    
    public async Task CreateClientRoleAsync(UserRole role, string description = "")
    {
        string roleName = role.ToString();
        
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/admin/realms/{_realm}/clients/{_clientUUID}/roles");
        var accessToken = await ObtainAdminToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var payload = new 
        {
            name = roleName,
            description = description
        };
        
        var jsonContent = JsonSerializer.Serialize(payload, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<List<RoleRepresentation>> GetClientRolesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/admin/realms/{_realm}/clients/{_clientUUID}/roles");
        
        var accessToken = await ObtainAdminToken();
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var responseJson = await response.Content.ReadAsStringAsync();
        var roles = JsonSerializer.Deserialize<List<RoleRepresentation>>(responseJson, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });
    
        return roles ?? new List<RoleRepresentation>();
    }
    
    public async Task<RoleRepresentation?> GetClientRoleByNameAsync(string clientUuid, UserRole userRole)
    {
        string roleName = userRole.ToString();
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/admin/realms/{_realm}/clients/{clientUuid}/roles/{roleName}");
        var accessToken = await ObtainAdminToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    
        var responseJson = await response.Content.ReadAsStringAsync();
        var role = JsonSerializer.Deserialize<RoleRepresentation>(responseJson, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    
        return role;
    }
    
    public async Task AssignClientRolesAsync(string userId, IEnumerable<RoleRepresentation> roles)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/role-mappings/clients/{_clientUUID}");

        var accessToken = await ObtainAdminToken();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var payload = JsonSerializer.Serialize(roles, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task AssignClientRoleAsync(string userId, RoleRepresentation role)
    {
        await AssignClientRolesAsync(userId, new[] { role });
    }
    
    public async Task<UserRepresentation?> GetUserByEmailAsync(string email)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/admin/realms/{_realm}/users?email={Uri.EscapeDataString(email)}");

        var accessToken = await ObtainAdminToken();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        
        var users = JsonSerializer.Deserialize<List<UserRepresentation>>(responseJson, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        return users?.FirstOrDefault();
    }

    public async Task<TokenResponse> LoginAsync(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token");

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        });

        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    
        if (tokenResponse == null)
        {
            throw new Exception("Failed to deserialize token response.");
        }
    
        return tokenResponse;
    }
    
    public async Task LogoutAsync(string refreshToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/logout");

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });

        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<TokenResponse> RequestNewTokensAsync(string refreshToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token");

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });

        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    
        if (tokenResponse == null)
        {
            throw new Exception("Failed to deserialize token response.");
        }
    
        return tokenResponse;
    }
    
    private async Task<string> ObtainAdminToken()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token");
        
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret)
        });

        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<ClientTokenResponse>(responseJson);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new Exception("Failed to obtain admin token from Keycloak: Access token is null or empty.");
        }

        return tokenResponse.AccessToken;
    }
}