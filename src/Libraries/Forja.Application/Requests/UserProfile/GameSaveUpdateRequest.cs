namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to update a game save in the user's profile.
/// </summary>
/// <remarks>
/// A <see cref="GameSaveUpdateRequest"/> is used to encapsulate the data required to update
/// an existing game save file in the system. This includes information such as the unique
/// identifier of the save, the name of the save file, the owner/user ID, the associated
/// game in the user's library, the URL of the save file, and the last updated timestamp.
/// </remarks>
public class GameSaveUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the game save update request.
    /// </summary>
    /// <remarks>
    /// This property is required and represents a globally unique identifier (GUID)
    /// associated with the specific game save update.
    /// </remarks>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the name of the game save.
    /// This property is required and its maximum length is restricted to 100 characters.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the unique identifier of the user associated with the game save update request.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the game within the user's library.
    /// </summary>
    [Required]
    public Guid UserLibraryGameId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the saved game file.
    /// </summary>
    /// <remarks>
    /// This property is required and must be a valid URL. It represents the location
    /// where the saved game file is stored.
    /// </remarks>
    [Required]
    [Url]
    public string SaveFileUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the timestamp indicating the last time the game save was updated.
    /// </summary>
    /// <remarks>
    /// This property is required and is used to track when the game save was last modified.
    /// </remarks>
    [Required]
    public DateTime? LastUpdatedAt { get; set; }
}