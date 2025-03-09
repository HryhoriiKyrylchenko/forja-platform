namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request object for creating a new game save.
/// </summary>
/// <remarks>
/// This class is used to encapsulate the information required to create a new game save
/// for a user, including metadata such as the name, associated user and game identifiers,
/// and the URL of the save file.
/// </remarks>
public class GameSaveCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the game save. This property is required and has a maximum length of 100 characters.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the user associated with the game save.
    /// </summary>
    /// <remarks>
    /// This property is required and must be assigned a valid GUID value.
    /// </remarks>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents the unique identifier of the game in the user's library.
    /// This property is required for creating a game save and ensures that
    /// the save is associated with a specific game owned by the user.
    /// </summary>
    [Required]
    public Guid UserLibraryGameId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the save file.
    /// This property is required and must conform to a valid URL format.
    /// </summary>
    [Required]
    [Url]
    public string SaveFileUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the game save was created.
    /// </summary>
    /// <remarks>
    /// This property is required and should represent the exact timestamp
    /// of the game's save creation.
    /// </remarks>
    [Required]
    public DateTime CreatedAt { get; set; }
}