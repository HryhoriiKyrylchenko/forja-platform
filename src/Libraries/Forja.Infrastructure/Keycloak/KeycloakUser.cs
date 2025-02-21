/// <summary>
/// Represents a user in Keycloak.
/// </summary>
public class KeycloakUser
{
    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}