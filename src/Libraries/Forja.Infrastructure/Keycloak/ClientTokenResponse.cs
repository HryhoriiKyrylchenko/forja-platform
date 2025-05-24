namespace Forja.Infrastructure.Keycloak;

/// <summary>
/// Represents the response received from Keycloak when requesting a client token.
/// </summary>
public class ClientTokenResponse
{
    /// <summary>
    /// Gets or sets the access token issued by Keycloak.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the token issued.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of seconds until the access token expires.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the number of seconds until the refresh token expires.
    /// </summary>
    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the not-before policy value.
    /// </summary>
    [JsonPropertyName("not-before-policy")]
    public int NotBeforePolicy { get; set; }

    /// <summary>
    /// Gets or sets the scope of the access token.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;
}