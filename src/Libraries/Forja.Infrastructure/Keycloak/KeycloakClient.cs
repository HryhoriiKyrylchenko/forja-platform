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
        
        var passwordValidationResult = PasswordValidator.ValidatePassword(user.Password);
        if (!passwordValidationResult.IsValid)
        {
            throw new ArgumentException(passwordValidationResult.ErrorMessage);
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
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm))
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

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/admin/realms/{_realm}/roles");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new
            {                name = role,

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
            throw new Exception($"Failed to create realm role. Status: {response.StatusCode}, Response: {responseContent}");
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
        await AssignRolesAsync(userId, [role]);
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
        await DeleteClientRolesAsync(userId, [role]);
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
        request.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        ]);
        
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
        request.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        ]);
        
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
        request.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        ]);

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
        request.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret)
        ]);

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
    
    // /////////////////////////////
    /// <inheritdoc />
    public async Task ChangePasswordAsync(string keycloakUserId, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl)
            || string.IsNullOrWhiteSpace(_realm))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(keycloakUserId));
        }
        
        var passwordValidationResult = PasswordValidator.ValidatePassword(newPassword);
        if (!passwordValidationResult.IsValid)
        {
            throw new ArgumentException(passwordValidationResult.ErrorMessage);
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
        
        var requestUrl = $"{_baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}/reset-password";
    
        var requestBody = new
        {
            type = "password",
            value = newPassword,
            temporary = false 
        };
    
        var request = new HttpRequestMessage(HttpMethod.Put, requestUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };
    
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        response.EnsureSuccessStatusCode();
    }
    
    /// <inheritdoc />
    public async Task EnableTwoFactorAuthenticationAsync(string keycloakUserId)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl)
            || string.IsNullOrWhiteSpace(_realm))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(keycloakUserId));
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
        
        var requestUrl = $"{_baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}";
        
        var requestBody = new
        {
            requiredActions = new[] { "CONFIGURE_TOTP" }
        };
        
        var request = new HttpRequestMessage(HttpMethod.Put, requestUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        response.EnsureSuccessStatusCode();
    }
    
    /// <inheritdoc />
    public async Task DisableTwoFactorAuthenticationAsync(string keycloakUserId)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl)
            || string.IsNullOrWhiteSpace(_realm))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(keycloakUserId));
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
        
        var url = $"{_baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}/credentials";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        response.EnsureSuccessStatusCode();
        
        var credentials = JsonSerializer.Deserialize<List<dynamic>>(await response.Content.ReadAsStringAsync());
        
        if (credentials == null)
        {
            throw new Exception("Failed to deserialize credentials.");
        }
        
        foreach (var credential in credentials)
        {
            if (credential["type"].ToString() == "otp") 
            {
                var deleteUrl = $"{url}/{credential["id"]}";
                var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, deleteUrl);
                deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
                var deleteResponse = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
                deleteResponse.EnsureSuccessStatusCode();
            }
        }
    }
    
    /// <inheritdoc />
    public async Task<bool> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) 
            || string.IsNullOrWhiteSpace(_realm)
            || string.IsNullOrWhiteSpace(_clientId)
            || string.IsNullOrWhiteSpace(_clientSecret))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be null or whitespace.", nameof(token));
        }
        
        var introspectionUrl = $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token/introspect";
    
        var request = new HttpRequestMessage(HttpMethod.Post, introspectionUrl);
        request.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("token", token),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret)
        ]);
        
        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        
        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("active", out var activeElement) && activeElement.ValueKind == JsonValueKind.False)
            {
                return false;
            }

            return true;
        }
        catch (JsonException ex)
        {
            throw new Exception("Error parsing token validation response.", ex);
        }
    }
    
    /// <inheritdoc />
    public async Task ResetUserPasswordAsync(string userId, string newPassword, bool temporary = false)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_realm))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(userId));
        }
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ArgumentException("New password cannot be null or whitespace.", nameof(newPassword));
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
    
        var url = $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/reset-password";
    
        var credentialPayload = new {
            type = "password",
            value = newPassword,
            temporary
        };
    
        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(credentialPayload)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    
    /// <inheritdoc />
    public async Task EnableDisableUserAsync(string keycloakUserId, bool enable)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl)
            || string.IsNullOrWhiteSpace(_realm))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }

        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(keycloakUserId));
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
        
        var requestUrl = $"{_baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}";
    
        var requestBody = new
        {
            enabled = enable
        };

        var request = new HttpRequestMessage(HttpMethod.Put, requestUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task ConfirmUserEmailAsync(string keycloakUserId)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl)
            || string.IsNullOrWhiteSpace(_realm))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }

        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(keycloakUserId));
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
        
        var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}")
        {
            Content = new StringContent(JsonSerializer.Serialize(new { emailVerified = true }), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);
        response.EnsureSuccessStatusCode();
    }
    
    /// <inheritdoc />
    [Obsolete("This method uses Keycloak UI to send the required action email. It is deprecated and will be removed in a future release.")]
    public async Task SendRequiredActionEmailAsync(
        string keycloakUserId,
        IEnumerable<string> actions,
        int lifespan = 3600,
        string? redirectUri = null)
    {
        if (string.IsNullOrWhiteSpace(_baseUrl)
            || string.IsNullOrWhiteSpace(_realm)
            || string.IsNullOrWhiteSpace(_clientId))
        {
            throw new InvalidOperationException("KeycloakClient is not properly initialized.");
        }

        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User Id cannot be null or whitespace.", nameof(keycloakUserId));
        }

        var actionsList = actions.ToList();
        if (!actionsList.Any())
        {
            throw new ArgumentException("Actions cannot be empty.", nameof(actions));
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
        
        var url = $"{_baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}/execute-actions-email";

        var queryParams = new List<string>
        {
            $"client_id={_clientId}",
            $"lifespan={lifespan}"
        };

        if (!string.IsNullOrWhiteSpace(redirectUri))
        {
            queryParams.Add($"redirect_uri={Uri.EscapeDataString(redirectUri)}");
        }

        if (queryParams.Any())
        {
            url += "?" + string.Join("&", queryParams);
        }

        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(actionsList)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpRetryHelper.ExecuteWithRetryAsync(_httpClient, request);

        response.EnsureSuccessStatusCode();
    }
    
    /// <inheritdoc />
    public string GetKeycloakUserId(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        return userId ?? throw new Exception("User ID (sub) not found in access token");
    }
}