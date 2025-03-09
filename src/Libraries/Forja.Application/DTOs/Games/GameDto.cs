namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object (DTO) for a game within the application.
/// </summary>
public class GameDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the game.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the game.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the game, providing a concise summary of its key features or theme.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the game.
    /// Provides additional information about the game, including its storyline, features, or other relevant details.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the developer of the game.
    /// </summary>
    public string Developer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the minimal age required to access or play the game.
    /// This property is used to enforce age-based restrictions based on
    /// the content or suitability of the game for younger audiences.
    /// </summary>
    public string MinimalAge { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the platforms on which the game is available.
    /// This property typically contains a list of supported platforms,
    /// such as PC, PlayStation, Xbox, or others in a string format.
    /// </summary>
    public string Platforms { get; set; } = string.Empty;

    /// <summary>
    /// Represents the price of the game.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the game's logo.
    /// </summary>
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release date of the game.
    /// </summary>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Indicates whether the game is currently active or available.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the languages supported for the game's user interface.
    /// </summary>
    public string InterfaceLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of audio languages available for the game.
    /// </summary>
    public string AudioLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the available subtitles languages for the game.
    /// </summary>
    public string SubtitlesLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the system requirements for the game.
    /// Represents the hardware and software specifications
    /// needed to run the game.
    /// </summary>
    public string? SystemRequirements { get; set; }

    /// <summary>
    /// Gets or sets the URL where the game's storage or related files are located.
    /// This property can be null if no storage URL is specified.
    /// </summary>
    public string? StorageUrl { get; set; }
}