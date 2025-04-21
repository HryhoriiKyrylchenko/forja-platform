namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a game file associated with a specific game version.
/// </summary>
[Table("GameFiles", Schema = "games")]
public class GameFile
{
    /// <summary>
    /// Gets or sets the unique identifier for the object.
    /// This property is used as a primary key or reference to distinguish
    /// the object instance from others within its context or collection.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the associated game version.
    /// </summary>
    [ForeignKey("GameVersion")]
    public Guid GameVersionId { get; set; }

    /// <summary>
    /// Gets or sets the name of the file associated with the game version.
    /// </summary>
    /// <remarks>
    /// This property represents the name of the game file, including its extension.
    /// It is used to identify the file within the context of its associated game version.
    /// </remarks>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relative or absolute path of the game file location.
    /// This property is used to reference where the file associated with the game version is stored.
    /// It is required for locating the file during processing or retrieval.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Represents the size of the game file in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the hash value of the file. This property is used to store a unique checksum or fingerprint
    /// to ensure file integrity and authenticity. It is typically calculated using a hashing algorithm.
    /// </summary>
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the game file is an archive file (e.g., a .zip file).
    /// This property is determined based on the file name and is primarily
    /// used to differentiate between archive files and other file types.
    /// </summary>
    public bool IsArchive { get; set; }

    /// <summary>
    /// Represents the version of the game associated with the current entity.
    /// </summary>
    /// <remarks>
    /// The <see cref="GameVersion"/> property establishes a relationship between the current entity and the
    /// corresponding game version in the system. It provides access to details such as version number, storage URL,
    /// file size, hash, changelog, release date, and associated game entity.
    /// </remarks>
    public virtual GameVersion GameVersion { get; set; } = null!;
}