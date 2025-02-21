/// <summary>
/// Represents the response received from Keycloak when requesting a token.
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// Gets or sets the access token issued by Keycloak.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the duration in seconds for which the access token is valid.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the duration in seconds for which the refresh token is valid.
    /// </summary>
    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the refresh token issued by Keycloak.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the token issued.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the not-before policy value.
    /// </summary>
    [JsonPropertyName("not-before-policy")]
    public int NotBeforePolicy { get; set; }

    /// <summary>
    /// Gets or sets the session state associated with the token.
    /// </summary>
    [JsonPropertyName("session_state")]
    public string SessionState { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the scope of the token.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;
}