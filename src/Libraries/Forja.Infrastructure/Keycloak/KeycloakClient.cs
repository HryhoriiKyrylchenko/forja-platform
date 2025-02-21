namespace Forja.Infrastructure.Keycloak;

/// <summary>
/// A client for interacting with the Keycloak API, designed to manage user authentication and roles.
/// </summary>
public class KeycloakClient : IKeycloakClient
{
    private readonly HttpClient _httpClient;
    private readonly string? _baseUrl;
    private readonly string? _realm;
    private readonly string? _clientId;
    private readonly string? _clientUuid;
    private readonly string? _clientSecret;
    
    public KeycloakClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["Keycloak:BaseUrl"];
        _realm = configuration["Keycloak:Realm"];
        _clientId = configuration["Keycloak:ClientId"];
        _clientUuid = configuration["Keycloak:ClientUUID"];
        _clientSecret = configuration["Keycloak:ClientSecret"];
    }
    
    /// <inheritdoc />
    public async Task<string> CreateUserAsync(KeycloakUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
        {
            throw new ArgumentException("Email and Password must be provided.");
        }
        
        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/admin/realms/{_realm}/users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new
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
            }, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create user. Status: {response.StatusCode}, Details: {errorDetails}");
        }
        
        if (response.Headers.Location == null)
        {
            throw new Exception("User created, but the Location header is missing. Cannot retrieve user ID.");
        }

        var userId = new Uri(response.Headers.Location.ToString()).Segments.Last();
        return userId;
    }

    /// <inheritdoc />
    public async Task CreateClientRoleAsync(string role, string description = "")
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm) || string.IsNullOrWhiteSpace(_clientUuid))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }

        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role name cannot be null or whitespace.", nameof(role));
        }
        
        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/admin/realms/{_realm}/clients/{_clientUuid}/roles");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new
            {
                name = role,
                description
            }, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create client role. Status: {response.StatusCode}, Response: {responseContent}");
        }
    }


    /// <inheritdoc />
    public async Task<List<RoleRepresentation>> GetClientRolesAsync()
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm) ||
            string.IsNullOrWhiteSpace(_clientUuid))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }

        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Get, 
            $"{_baseUrl}/admin/realms/{_realm}/clients/{_clientUuid}/roles");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to obtain client roles. Status: {response.StatusCode}, Response: {responseContent}");
        }
        
        List<RoleRepresentation>? roles;
        try
        {
            roles = JsonSerializer.Deserialize<List<RoleRepresentation>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            throw new Exception("Failed to deserialize Keycloak roles from the server response.", ex);
        }
        return roles ?? new List<RoleRepresentation>();
    }

    /// <inheritdoc />
    public async Task<RoleRepresentation?> GetClientRoleByNameAsync(string userRole)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm) || string.IsNullOrWhiteSpace(_clientUuid))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Get, 
            $"{_baseUrl}/admin/realms/{_realm}/clients/{_clientUuid}/roles/{userRole}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);

        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to obtain client role. Status: {response.StatusCode}, Response: {responseJson}");
        }

        RoleRepresentation? role;
        try
        {
            role = JsonSerializer.Deserialize<RoleRepresentation>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            throw new Exception("Failed to deserialize Keycloak role from the server response.", ex);
        }
    
        return role;
    }

    /// <inheritdoc />
    public async Task<List<RoleRepresentation>> GetUserRolesAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm) || string.IsNullOrWhiteSpace(_clientUuid))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(userId));
        }
        
        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Get, 
            $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/role-mappings/clients/{_clientUuid}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to obtain user roles. Status: {response.StatusCode}, Response: {responseJson}");
        }
        
        List<RoleRepresentation>? roles;
        try
        {
            roles = JsonSerializer.Deserialize<List<RoleRepresentation>>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            throw new Exception("Failed to deserialize Keycloak roles from the server response.", ex);
        }

        return roles ?? new List<RoleRepresentation>();
    }

    /// <inheritdoc />
    public async Task<bool> CheckUserRoleAsync(string userId, string userRole)
    {
        var roles = await GetUserRolesAsync(userId);

        return roles.Any(role => role.Name.Equals(userRole, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public async Task AssignRolesAsync(string userId, IEnumerable<RoleRepresentation> roles)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm) || string.IsNullOrWhiteSpace(_clientUuid))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(userId));
        }

        var roleRepresentations = roles.ToList();
        if (roles == null || !roleRepresentations.Any())
        {
            throw new ArgumentException("Roles cannot be null or empty.", nameof(userId));
        }
        
        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Post, 
            $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/role-mappings/clients/{_clientUuid}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(
            JsonSerializer.Serialize(roleRepresentations,
            new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            }),
            Encoding.UTF8,
            "application/json"
        );
        
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to assign roles to user. Status: {response.StatusCode}, Response: {responseContent}");
        }
    }

    /// <inheritdoc />
    public async Task AssignRoleAsync(string userId, RoleRepresentation role)
    {
        await AssignRolesAsync(userId, new[] { role });
    }

    /// <inheritdoc />
    public async Task DeleteClientRolesAsync(string userId, IEnumerable<RoleRepresentation> roles)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm) || string.IsNullOrWhiteSpace(_clientUuid))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(userId));
        }

        var roleRepresentations = roles.ToList();
        if (roles == null || !roleRepresentations.Any())
        {
            throw new ArgumentException("Roles cannot be null or empty.", nameof(userId));
        }
        
        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Delete, 
            $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/role-mappings/clients/{_clientUuid}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(
            JsonSerializer.Serialize(roleRepresentations,
            new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete roles from user. Status: {response.StatusCode}, Response: {responseContent}");
        }
    }

    /// <inheritdoc />
    public async Task DeleteClientRoleAsync(string userId, RoleRepresentation role)
    {
        await DeleteClientRolesAsync(userId, new[] { role });
    }

    /// <inheritdoc />
    public async Task<UserRepresentation?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));
        }
        
        string accessToken;
        try
        {
            accessToken = await ObtainAdminToken();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Failed to obtain admin token.", ex);
        }
        
        var request = new HttpRequestMessage(HttpMethod.Get, 
            $"{_baseUrl}/admin/realms/{_realm}/users?email={Uri.EscapeDataString(email)}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get the user data. Status: {response.StatusCode}, Response: {responseJson}");
        }
        
        List<UserRepresentation>? users;
        try
        {
            users = JsonSerializer.Deserialize<List<UserRepresentation>>(responseJson, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            throw new Exception("Failed to deserialize Keycloak roles from the server response.", ex);
        }
        
        return users?.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<TokenResponse> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) 
            || string.IsNullOrWhiteSpace(_realm)
            || string.IsNullOrWhiteSpace(_clientId)
            || string.IsNullOrWhiteSpace(_clientSecret))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or whitespace.", nameof(username));
        }
        
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or whitespace.", nameof(password));
        }
        
        var request = new HttpRequestMessage(HttpMethod.Post, 
            $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        });
        
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);

        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to login. Status: {response.StatusCode}, Response: {responseJson}");
        }
        
        TokenResponse? tokenResponse;
        try
        {
            tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseJson, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            throw new Exception("Failed to deserialize Keycloak roles from the server response.", ex);
        }
    
        if (tokenResponse == null)
        {
            throw new Exception("Failed to deserialize token response.");
        }
    
        return tokenResponse;
    }

    /// <inheritdoc />
    public async Task LogoutAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) 
            || string.IsNullOrWhiteSpace(_realm)
            || string.IsNullOrWhiteSpace(_clientId)
            || string.IsNullOrWhiteSpace(_clientSecret))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be null or whitespace.", nameof(refreshToken));
        }
        
        var request = new HttpRequestMessage(HttpMethod.Post, 
            $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/logout");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });
        
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);

        
        if (!response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to logout. Status: {response.StatusCode}, Response: {responseJson}");
        }
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RequestNewTokensAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) 
            || string.IsNullOrWhiteSpace(_realm)
            || string.IsNullOrWhiteSpace(_clientId)
            || string.IsNullOrWhiteSpace(_clientSecret))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be null or whitespace.", nameof(refreshToken));
        }
        
        var request = new HttpRequestMessage(HttpMethod.Post, 
            $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);

        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to request new token. Status: {response.StatusCode}, Response: {responseJson}");
        }
        
        TokenResponse? tokenResponse;
        try
        {
            tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseJson, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            throw new Exception("Failed to deserialize Keycloak roles from the server response.", ex);
        }
    
        if (tokenResponse == null)
        {
            throw new Exception("Failed to deserialize token response.");
        }
    
        return tokenResponse;
    }


    /// Obtains an administrative access token from the Keycloak server using client credentials.
    /// Throws an InvalidOperationException if the client is not properly initialized, or other exceptions if the request fails.
    /// <returns>A string representing the access token.</returns>
    private async Task<string> ObtainAdminToken()
    {
        if (string.IsNullOrWhiteSpace(_baseUrl)
            || string.IsNullOrWhiteSpace(_realm)
            || string.IsNullOrWhiteSpace(_clientId)
            || string.IsNullOrWhiteSpace(_clientSecret))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, 
            $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret)
        });

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to obtain admin token. Status: {response.StatusCode}, Details: {responseJson}");
        }

        ClientTokenResponse? tokenResponse;
        try
        {
            tokenResponse = JsonSerializer.Deserialize<ClientTokenResponse>(responseJson);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Failed to parse token response. Response: {responseJson}", ex);
        }

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new Exception("Failed to authenticate with Keycloak.");
        }

        return tokenResponse.AccessToken;
    }
}