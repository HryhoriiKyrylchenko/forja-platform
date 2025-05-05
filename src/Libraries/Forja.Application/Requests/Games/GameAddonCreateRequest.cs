namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a data transfer object used for creating a new game addon.
/// </summary>
/// <remarks>
/// This class contains all the necessary information required to create a new game addon,
/// including title, description, developer information, minimal age restrictions, supported platforms,
/// pricing, language support, game association, and storage details. Some fields are marked as required
/// to ensure that the creation process meets the necessary requirements.
/// </remarks>
public class GameAddonCreateRequest
{
    /// <summary>
    /// Represents the title of the game addon being created.
    /// This property is required and must not be empty or contain only whitespace.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief overview or summary of the game addon content.
    /// This is a short descriptive text providing essential details about the addon.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Provides a detailed description of the game addon.
    /// This property is used to store comprehensive textual information
    /// describing the content and features of the game addon.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents a request for creating a game add-on.
    /// Contains detailed information necessary to create a new game add-on, including title, description, developer,
    /// minimal age requirement, platform compatibility, price, and languages.
    /// </summary>
    public string Developer { get; set; } = string.Empty;

    /// <summary>
    /// Represents the minimum age rating for content in the system.
    /// This enumeration defines specific age classifications that are required for age-related restrictions
    /// and compliance with applicable standards or regulations.
    /// </summary>
    [Required]
    public int MinimalAge { get; set; }

    /// <summary>
    /// Represents the platforms where the game or addon is available or supported.
    /// </summary>
    public List<PlatformType> Platforms { get; set; } = [];

    /// <summary>
    /// Represents the price of a game addon or product.
    /// <para>
    /// This property defines the monetary value associated with the game addon
    /// or product within the system. It is a decimal value to accommodate
    /// precision for currency-related calculations.
    /// </para>
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo associated with the game addon.
    /// </summary>
    /// <remarks>
    /// This property contains the link to the logo, which can be used to represent the game addon visually.
    /// </remarks>
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the release date of the game addon.
    /// This property is required and indicates the date
    /// when the game addon becomes or became available.
    /// </summary>
    [Required]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Indicates whether the game addon is currently active or available for use.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the supported interface languages for the game addon.
    /// This property expects a string that represents the available languages
    /// (e.g., in a comma-separated format such as "English, French, German").
    /// </summary>
    public string InterfaceLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the languages available for audio in the game addon.
    /// This property specifies the languages in which audio is provided
    /// for the game or its addon, allowing developers to indicate audio
    /// localization options for end users.
    /// </summary>
    public string AudioLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subtitle languages available for the game addon.
    /// </summary>
    /// <remarks>
    /// This property specifies the languages available for subtitles in the game addon.
    /// It is represented as a comma-separated string of language codes or names.
    /// Commonly used to inform users of the subtitle options included in the game addon.
    /// </remarks>
    public string SubtitlesLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Represents the unique identifier of the game associated with this addon.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the storage location for the game add-on.
    /// </summary>
    /// <remarks>
    /// This property may contain a null value if no storage URL is specified.
    /// Used to reference external resources associated with the game add-on.
    /// </remarks>
    public string? StorageUrl { get; set; }
}