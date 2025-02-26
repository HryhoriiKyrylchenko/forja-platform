namespace Forja.Application.DTOs.UserRegistration;

/// <summary>
/// Represents a command used to create a new role in the system.
/// </summary>
/// <remarks>
/// This class contains the necessary details, such as the role name and an optional description,
/// required for creating a new role within the application.
/// </remarks>
public class CreateRoleCommand
{
    /// <summary>
    /// Gets or sets the name of the role to be created.
    /// </summary>
    /// <remarks>
    /// This property represents the unique identifier for the role being defined.
    /// It cannot be null or empty and should adhere to naming conventions for roles.
    /// </remarks>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the role being created.
    /// This property provides additional information about the purpose or usage of the role. Defaults to an empty string.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}