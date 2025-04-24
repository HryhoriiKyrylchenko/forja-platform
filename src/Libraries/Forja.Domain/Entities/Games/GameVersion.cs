namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a version of a game.
/// </summary>
/// <remarks>
/// The <see cref="GameVersion"/> class contains information about a specific version of a game,
/// including its version number, storage location, file size, and release details.
/// It is associated with a <see cref="Game"/> entity and can have a collection of related <see cref="GameFile"/> entities.
/// </remarks>
[Table("GameVersions", Schema = "games")]
public class GameVersion
{
    /// <summary>
    /// Gets or sets the unique identifier for an entity or object.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the associated game.
    /// </summary>
    /// <remarks>
    /// This property is a foreign key linking the game version to its corresponding game.
    /// It ensures referential integrity by associating the version-specific data to a game entity.
    /// </remarks>
    [ForeignKey("Game")]
    public Guid GameId { get; set; }

    /// <summary>
    /// Represents the version identifier of a specific game release.
    /// </summary>
    /// <remarks>
    /// The version is a string with a maximum length of 15 characters and uniquely identifies a game release.
    /// It is primarily used to differentiate between multiple versions of the same game.
    /// </remarks>
    [MaxLength(15)]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL for the storage location of the game version file.
    /// </summary>
    /// <remarks>
    /// The URL typically points to a resource where the game version artifacts
    /// (such as installation files or updates) are stored and can be accessed for download.
    /// </remarks>
    public string StorageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the size of the game file in bytes.
    /// </summary>
    /// <remarks>
    /// This property is used to store the file size of a game version, which must be a positive value.
    /// </remarks>
    public long FileSize { get; set; }

    /// <summary>
    /// Represents the hash value of the game file associated with a specific game version.
    /// </summary>
    /// <remarks>
    /// The hash property is used to ensure data integrity and verify that the contents of the file
    /// have not been altered. This is typically a cryptographic hash (e.g., SHA256) of the file's binary data.
    /// </remarks>
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Represents the changelog or release notes associated with a specific game version.
    /// This property documents changes, fixes, updates, or new features introduced in the game version.
    /// </summary>
    public string Changelog { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release date of the game version.
    /// </summary>
    /// <remarks>
    /// Represents the DateTime when the specific version of the game was released.
    /// This value is expected to be in the past and validated accordingly.
    /// </remarks>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Represents a game entity within the system. Inherits from the <see cref="Product"/> class.
    /// </summary>
    /// <remarks>
    /// The <see cref="Game"/> class is designed to encapsulate information about individual games,
    /// including but not limited to system requirements, playtime tracking, and associated collections
    /// of addons, tags, mechanics, versions, patches, and achievements. It provides a comprehensive
    /// structure for managing game-related data in a domain-driven context.
    /// </remarks>
    public virtual Game Game { get; set; } = null!;

    /// <summary>
    /// Represents the collection of files associated with a specific game version.
    /// </summary>
    /// <remarks>
    /// The <see cref="Files"/> property contains a collection of <see cref="GameFile"/> instances,
    /// which represent individual files tied to the game version. Each file contains information
    /// such as file name, file path, size, hash, and whether it is an archive.
    /// </remarks>
    public virtual ICollection<GameFile> Files { get; set; } = [];
}