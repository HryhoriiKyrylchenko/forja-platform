namespace Forja.Application.Requests.Authentication;

/// <summary>
/// Represents a request for enabling or disabling a user account.
/// </summary>
public class EnableDisableUserRequest
{
    /// <summary>
    /// Gets or sets a value indicating whether the user account should be enabled or disabled.
    /// </summary>
    /// <remarks>
    /// A value of <c>true</c> enables the user account, while a value of <c>false</c> disables it.
    /// This property is used as part of the request to toggle the user's status in the system.
    /// </remarks>
    public bool Enable { get; set; }

    /// <summary>
    /// Represents the unique identifier of a user in the Keycloak identity provider.
    /// </summary>
    /// <remarks>
    /// This property is used to perform operations on a specific user in the system,
    /// such as enabling or disabling the user account.
    /// </remarks>
    public string KeycloakUserId { get; set; } = string.Empty;
}