namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request model for creating a new game, containing all necessary and optional data.
/// </summary>
public class GameCreateRequest
{
    /// <summary>
    /// Represents the title of the game to be created.
    /// </summary>
    /// <remarks>
    /// This property is required for the creation of a game and must contain a valid, non-empty string.
    /// </remarks>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the game, providing a concise overview or summary.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Represents a detailed description of the game.
    /// </summary>
    /// <remarks>
    /// This property provides an extended narrative or overview of the game,
    /// which can include features, storyline, or other descriptive elements
    /// that give players insights into the content and nature of the game.
    /// It is optional and defaults to an empty string if not specified.
    /// </remarks>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents the name of the developer or studio responsible for creating the game.
    /// </summary>
    public string Developer { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the minimum required age to access or play the game.
    /// </summary>
    /// <remarks>
    /// This property is mandatory and must be defined as a positive integer.
    /// It ensures the content is suitable for users above the specified age.
    /// </remarks>
    [Required]
    public int MinimalAge { get; set; }

    /// <summary>
    /// Gets or sets the platforms on which the game is available.
    /// This property represents a string that specifies the supported platforms
    /// (e.g., PC, Xbox, PlayStation).
    /// </summary>
    public string Platforms { get; set; } = string.Empty;

    /// <summary>
    /// Represents the price of the game.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo associated with the game.
    /// </summary>
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the release date of the game. This property is required and must represent a valid date.
    /// </summary>
    [Required]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Indicates whether the game is active or not.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the interface languages available for the game.
    /// </summary>
    /// <remarks>
    /// This property specifies the languages supported by the game interface,
    /// such as menu text, UI elements, or in-game instructions.
    /// </remarks>
    public string InterfaceLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Represents the available audio languages for the game.
    /// This property defines the list of languages supported within the game's audio content.
    /// </summary>
    public string AudioLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the languages available for subtitles in the game.
    /// </summary>
    /// <remarks>
    /// This property contains a list or description of languages that are supported for subtitles.
    /// It is helpful for players who require textual translations of in-game audio or dialogue.
    /// </remarks>
    public string SubtitlesLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Represents the system requirements necessary to run the game.
    /// This property may include details such as hardware specifications,
    /// software dependencies, or any other prerequisites required for proper functionality.
    /// </summary>
    public string? SystemRequirements { get; set; }
}