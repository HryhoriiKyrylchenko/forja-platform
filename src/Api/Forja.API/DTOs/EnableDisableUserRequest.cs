namespace Forja.API.DTOs;

public class EnableDisableUserRequest
{
    public bool Enable { get; set; }
    public string KeycloakUserId { get; set; } = string.Empty;
}