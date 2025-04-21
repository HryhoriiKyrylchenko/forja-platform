namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a patch for a game, including metadata such as version, file details, and release date.
/// </summary>
[Table("GamePatches", Schema = "games")]
public class GamePatch
{
    /// <summary>
    /// Represents the unique identifier for an entity or object.
    /// This property is typically used to distinguish instances
    /// within a system or data store.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the game associated with the patch.
    /// </summary>
    /// <remarks>
    /// This property serves as a foreign key linking the game patch to the corresponding game in the system.
    /// </remarks>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the name of the game patch.
    /// </summary>
    /// <remarks>
    /// The name uniquely identifies the game patch along with the associated game ID.
    /// This property is required and cannot be empty or whitespace.
    /// </remarks>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the version of the game from which the patch applies.
    /// </summary>
    /// <remarks>
    /// This property is utilized to define the starting version number of the game that the patch updates.
    /// It is used in processes such as patch creation, updates, and validation to ensure compatibility.
    /// </remarks>
    public string FromVersion { get; set; } = string.Empty;

    /// <summary>
    /// Represents the version that the game is updated to when the patch is applied.
    /// </summary>
    /// <remarks>
    /// This property specifies the target version of the game after applying the patch.
    /// It is used to determine the intended final state of the game version post-update.
    /// </remarks>
    public string ToVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL that points to the downloadable patch file for the game.
    /// This is typically used to provide a direct link from which the game update can be acquired.
    /// </summary>
    public string PatchUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the size of the game patch file in bytes.
    /// </summary>
    /// <remarks>
    /// This property is used to specify or retrieve the file size of a game patch.
    /// It must be greater than zero to be valid.
    /// </remarks>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the hash value of the game patch.
    /// Represents a unique identifier for the patch's file integrity or contents.
    /// Used for validation and ensuring the correctness of the patch file during updates or downloads.
    /// </summary>
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release date of the game patch.
    /// </summary>
    /// <remarks>
    /// This property indicates the date and time when the game patch became available for release.
    /// It is used to schedule or track patch deployments and can be essential for update management.
    /// </remarks>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Represents a game entity and inherits from the <see cref="Product"/> class.
    /// </summary>
    /// <remarks>
    /// The Game class provides properties and collections relevant to gaming, such as:
    /// - System requirements for running the game.
    /// - Total time played recorded for a game.
    /// - Relationships to associated game addons, tags, mechanics, versions, patches, and achievements.
    /// </remarks>
    public virtual Game Game { get; set; } = null!;
}