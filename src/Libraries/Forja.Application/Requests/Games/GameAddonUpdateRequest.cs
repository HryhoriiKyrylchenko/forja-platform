namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update an existing game add-on.
/// </summary>
public class GameAddonUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the game addon update request.
    /// </summary>
    /// <remarks>
    /// This property is required and is used to uniquely identify the game addon
    /// during the update process.
    /// </remarks>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the title of the game addon.
    /// This property is required and must contain a meaningful value.
    /// It is used to identify and display the name of the game addon in various contexts.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Represents a brief overview or summary of the game addon.
    /// This property is optional and provides a concise description
    /// of the addon for display purposes.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Represents a descriptive text providing detailed information or explanation
    /// about the game addon. This property is intended to contain a more extensive and thorough description
    /// compared to the short description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the developer of the game addon.
    /// Represents the name or organization responsible for creating the game addon.
    /// </summary>
    public string Developer { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the minimum age required to access or use certain content, features, or services.
    /// </summary>
    /// <remarks>
    /// The value of this property is represented as an enumeration of predefined age categories.
    /// It is generally applied to ensure content suitability based on user age or to comply with
    /// regulatory and age-related restrictions.
    /// </remarks>
    [Required]
    public int MinimalAge { get; set; }

    /// <summary>
    /// Represents the platforms on which the game addon is available.
    /// This property is used to specify the supported platforms, such as Windows, Mac, Linux, or others.
    /// </summary>
    public List<PlatformType> Platforms { get; set; } = [];

    /// <summary>
    /// Represents the price of the game addon.
    /// </summary>
    /// <remarks>
    /// This property is used to store and manipulate the monetary value associated with a game addon.
    /// The price is defined as a decimal value to support precision in currency calculations.
    /// </remarks>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo associated with the game addon.
    /// </summary>
    /// <remarks>
    /// This property contains the path or address to the logo image file.
    /// It is used to represent the game's addon visually.
    /// </remarks>
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the release date of the game add-on. This property is required and must be set to a valid date.
    /// It is used to indicate when the game add-on is officially available.
    /// </summary>
    [Required]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Indicates whether the entity is currently active.
    /// This property can be used to manage the availability or status of the entity,
    /// such as enabling or disabling features or items.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the interface languages available for the game add-on.
    /// </summary>
    /// <remarks>
    /// This property is used to specify the languages available in the user interface of the game add-on.
    /// It is represented as a string and can include a list of supported languages.
    /// </remarks>
    public string InterfaceLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the available audio languages for the game addon.
    /// This property represents the languages in which the game addon
    /// provides audio content. It can include a single language or
    /// multiple languages separated by a delimiter.
    /// </summary>
    public string AudioLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the languages available for subtitles in the game addon.
    /// </summary>
    /// <remarks>
    /// This property defines the text-based subtitle languages available to users while playing the game.
    /// It can include multiple languages as a comma-separated string.
    /// </remarks>
    public string SubtitlesLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Represents the unique identifier of a game associated with the game add-on update request.
    /// This value is required and cannot be an empty GUID.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the URL to the storage location, where additional data or files for the game addon
    /// can be accessed.
    /// </summary>
    /// <remarks>
    /// This property can be null if no storage URL is associated with the game addon. It may represent
    /// a location in cloud storage or another server where files are hosted.
    /// </remarks>
    public string? StorageUrl { get; set; }
}