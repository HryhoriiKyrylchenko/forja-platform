namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for a game addon, encapsulating
/// details about the addon such as title, description, pricing,
/// supported platforms, release date, and related game information.
/// </summary>
public class GameAddonDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the game addon.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the game addon.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the game addon.
    /// This property provides a concise overview or summary of the addon contents.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the game addon.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the developer of the game addon.
    /// Represents the entity or individual responsible for creating or developing the game addon.
    /// </summary>
    public string Developer { get; set; } = string.Empty;

    /// <summary>
    /// Represents the minimum age requirement for the game Addon.
    /// Defines the age limit to provide guidance on suitable content access.
    /// </summary>
    public string MinimalAge { get; set; } = string.Empty;

    /// <summary>
    /// Represents the platforms on which the game add-on is available.
    /// </summary>
    public string Platforms { get; set; } = string.Empty;

    /// <summary>
    /// Represents the price of the game addon.
    /// This property specifies the cost associated with the game addon as a decimal value.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo associated with the game addon.
    /// </summary>
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release date of the game add-on.
    /// This property represents the date when the game add-on becomes or became available.
    /// </summary>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the game addon is currently active or available.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the languages supported for the user interface of the game addon.
    /// </summary>
    public string InterfaceLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the audio languages supported by the game addon.
    /// This property holds a value representing the languages available for audio output.
    /// </summary>
    public string AudioLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the languages available for subtitles in the game add-on.
    /// This property represents a comma-separated list of languages that are supported for subtitles.
    /// </summary>
    public string SubtitlesLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the associated game.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the URL for accessing the stored content associated with the game addon.
    /// </summary>
    public string? StorageUrl { get; set; }
}