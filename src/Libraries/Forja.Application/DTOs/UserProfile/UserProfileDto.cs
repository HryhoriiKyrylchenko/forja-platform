namespace Forja.Application.DTOs.UserProfile;

/// <summary>
/// Represents a Data Transfer Object (DTO) for a user profile.
/// </summary>
/// <remarks>
/// This class is used to encapsulate user profile data for transfer across different layers of the application.
/// It contains information such as the user's unique identifier, username, personal details, and contact information.
/// </remarks>
public class UserProfileDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the user profile.
    /// </summary>
    /// <remarks>
    /// This property represents the unique identifier associated with a specific user profile,
    /// typically used as a primary key or unique reference within the system.
    /// </remarks>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username associated with the user profile.
    /// </summary>
    /// <remarks>
    /// The username is a required property and uniquely identifies the user within the system.
    /// It is limited to a maximum of 30 characters.
    /// </remarks>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    /// <remarks>
    /// The first name is an optional property that represents the given name of the user.
    /// </remarks>
    public string? Firstname { get; set; }

    /// <summary>
    /// Gets or sets the lastname of the user.
    /// </summary>
    /// <remarks>
    /// This property represents the user's lastname and is optional.
    /// It can have a maximum length of 30 characters as per the domain entity constraints.
    /// </remarks>
    public string? Lastname { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    /// <remarks>
    /// This property represents the primary email address associated with the user.
    /// It is used as a unique identifier in some operations and must be in a valid email format.
    /// </remarks>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's phone number. This property is optional and may be null if not provided.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the URL of the user's avatar image.
    /// This property can be null if the user has not provided an avatar.
    /// </summary>
    public string? AvatarUrl { get; set; }
}