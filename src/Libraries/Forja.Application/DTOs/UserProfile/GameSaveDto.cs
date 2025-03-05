namespace Forja.Application.DTOs.UserProfile;

/// <summary>
/// A Data Transfer Object (DTO) that represents a game save in the system.
/// </summary>
/// <remarks>
/// This class is used to transfer game save data between layers of the application.
/// It includes details about the user associated with the save, the URL of the saved file,
/// the creation date, the last updated date, and related entities.
/// </remarks>
public class GameSaveDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the game save entry.
    /// This property is required and must represent a valid GUID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the URL of the save file associated with the game save data.
    /// This property is required and must contain a valid URL.
    /// </summary>
    public string SaveFileUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the date and time when the game save was created.
    /// This property is automatically set to the current UTC datetime upon initialization.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Represents the date and time when the game save was last updated.
    /// </summary>
    /// <remarks>
    /// This property is nullable, meaning it can have a value indicating the last modification date and time
    /// or remain null if the game save has never been updated after its initial creation.
    /// </remarks>
    public DateTime? LastUpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the game save.
    /// This property is required and should reference a valid user in the system.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier that links the game save to a specific
    /// game in the user's library.
    /// </summary>
    public Guid UserLibraryGameId { get; set; }
}