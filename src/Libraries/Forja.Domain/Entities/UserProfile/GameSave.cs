namespace Forja.Domain.Entities.UserProfile;

/// <summary>
/// Represents a saved game for a user within the system.
/// </summary>
/// <remarks>
/// A GameSave entity stores information about a user's saved progress in a specific game from their library. It includes
/// references to the associated user and the game within the user's library.
/// </remarks>
[Table("GameSaves", Schema = "user-profile")]
public class GameSave
{
    /// <summary>
    /// Gets or sets the unique identifier for the GameSave entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the user associated with the game save.
    /// </summary>
    /// <remarks>
    /// This property is a foreign key linking to the <see cref="User"/> entity.
    /// It establishes a relationship between a specific game save and the user who owns it.
    /// </remarks>
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the association between a user and a game in their library.
    /// Acts as a foreign key linking the game save data to a specific game owned by the user in the user's library.
    /// </summary>
    [ForeignKey("UserLibraryGame")]
    public Guid UserLibraryGameId { get; set; }

    /// <summary>
    /// Gets or sets the URL location of the saved game file associated with a user's game library.
    /// This property ensures the URL is valid and is required for saving the associated game data.
    /// </summary>
    [Required]
    [Url]
    public string SaveFileUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date and time for the game save.
    /// This property records when the game save was initially created and is set to UTC time.
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp indicating when the game save was last updated.
    /// This property is nullable, allowing for distinction between records that have
    /// been updated and those that have not been modified since creation.
    /// </summary>
    public DateTime? LastUpdatedAt { get; set; }

    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    /// <remarks>
    /// This entity includes properties such as unique identifiers, user profile details,
    /// and relationships to other entities like GameSaves, UserLibraryGame, and more.
    /// It is a part of the "user-profile" schema in the Forja domain.
    /// </remarks>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Represents a game owned by a user in their library, including its associated metadata and relationships.
    /// </summary>
    /// <remarks>
    /// A `UserLibraryGame` links a specific `Game` entity with a `User`, providing details such as purchase date,
    /// associated add-ons, and related game saves. This entity also inherits soft deletion functionality from
    /// `SoftDeletableEntity`.
    /// </remarks>
    public virtual UserLibraryGame UserLibraryGame { get; set; } = null!;
}