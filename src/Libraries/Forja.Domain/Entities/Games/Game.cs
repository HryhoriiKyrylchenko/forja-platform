namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a game entity in the system that inherits from the <see cref="Product"/> class.
/// </summary>
/// <remarks>
/// The <see cref="Game"/> class contains information specific to games, such as system requirements
/// and time played, as well as collections of related entities including addons, tags, mechanics,
/// versions, patches, and achievements.
/// </remarks>
[Table("Games", Schema = "games")]
public class Game : Product
{
    /// <summary>
    /// Gets or sets the system requirements for the game.
    /// </summary>
    /// <remarks>
    /// This property contains information about the minimum and recommended
    /// hardware and software requirements necessary to run the game.
    /// </remarks>
    public string? SystemRequirements { get; set; }

    /// <summary>
    /// Represents the total time a game has been played.
    /// Allows tracking of gameplay duration for individual games.
    /// </summary>
    public TimeSpan? TimePlayed { get; set; }

    /// <summary>
    /// Represents a collection of game addons associated with a specific game.
    /// </summary>
    /// <remarks>
    /// Game addons extend the base game by providing additional content or features.
    /// Each game addon is represented as an instance of the <see cref="GameAddon"/> class.
    /// </remarks>
    public virtual ICollection<GameAddon> GameAddons { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of game tags associated with the current game.
    /// </summary>
    /// <remarks>
    /// This property represents the relationship between a game and its associated tags.
    /// It allows for tagging mechanisms to categorize or describe the game.
    /// </remarks>
    public virtual ICollection<GameTag> GameTags { get; set; } = [];

    /// <summary>
    /// Represents the association between a user and games owned or accessed within their library.
    /// This class contains details about the user's ownership, playtime, and additional data related to a specific game.
    /// </summary>
    public virtual ICollection<UserLibraryGame> UserLibraryGames { get; set; } = [];

    /// <summary>
    /// Represents the collection of game mechanics associated with a specific game.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between the Game entity and its associated
    /// GameMechanic entities, signifying the various mechanics utilized in the game.
    /// It is implemented as a virtual ICollection to facilitate lazy loading when using an ORM
    /// such as Entity Framework.
    /// </remarks>
    public virtual ICollection<GameMechanic> GameMechanics { get; set; } = [];
    
    /// <summary>
    /// Represents the collection of patches associated with a game.
    /// </summary>
    /// <remarks>
    /// Each <see cref="GamePatch"/> object in the collection details a specific game update or patch,
    /// including information about the originating version, target version, release date,
    /// download URL, and other patch-specific properties.
    /// </remarks>
    public virtual ICollection<GamePatch> GamePatches { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of achievements associated with the game.
    /// </summary>
    /// <remarks>
    /// Each achievement represents a specific milestone or accomplishment related
    /// to the game. This property enables linking a game to its achievements and
    /// allows navigation and retrieval of related achievement entities.
    /// </remarks>
    public virtual ICollection<Achievement> Achievements { get; set; } = [];
}