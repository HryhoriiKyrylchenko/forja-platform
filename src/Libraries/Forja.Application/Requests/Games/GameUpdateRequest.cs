namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update an existing game record with relevant details.
/// </summary>
public class GameUpdateRequest
{
    /// <summary>
    /// Represents the unique identifier for a game update request.
    /// This property is required and must not be an empty GUID.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the title of the game.
    /// This property is required and must contain a valid non-whitespace string.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief, concise summary or synopsis of a game.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the game.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the developer responsible for the game.
    /// </summary>
    public string Developer { get; set; } = string.Empty;

    /// <summary>
    /// Represents the minimum recommended or required age for the target audience of a game.
    /// Defines age restrictions to ensure content is suitable for specific age groups.
    /// </summary>
    [Required]
    public int MinimalAge { get; set; }

    /// <summary>
    /// Gets or sets the platforms supported by the game.
    /// This property holds a string that represents the platforms
    /// (e.g., PC, PlayStation, Xbox) for which the game is available.
    /// </summary>
    public string Platforms { get; set; } = string.Empty;

    /// <summary>
    /// Represents the price of the game.
    /// This property defines the monetary value or cost associated with acquiring the game.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the game's logo image.
    /// </summary>
    /// <remarks>
    /// This property holds the web address pointing to the game's logo,
    /// which can be used for display purposes in the application's interface.
    /// </remarks>
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the release date of the game.
    /// </summary>
    /// <remarks>
    /// This property is required and indicates the date when the game is or was officially released.
    /// </remarks>
    [Required]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Represents the active status of a game.
    /// Indicates whether the game is currently available and operational.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the interface languages available for the game.
    /// </summary>
    /// <remarks>
    /// This property represents the list of interface languages supported by the game.
    /// It is expected to be a string containing language codes or names separated by
    /// an appropriate delimiter (e.g., comma or semicolon).
    /// </remarks>
    public string InterfaceLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the audio languages available for the game.
    /// </summary>
    /// <remarks>
    /// This property contains a list or description of languages supported for the game's audio.
    /// It may include multiple languages, separated by a specific delimiter, or a detailed description,
    /// depending on how it is serialized or formatted.
    /// </remarks>
    public string AudioLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the supported subtitle languages of the game.
    /// This property contains a list or description of the languages
    /// available for subtitles in the game. It is represented as a string.
    /// </summary>
    public string SubtitlesLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Represents the required system configurations needed to run the game.
    /// </summary>
    /// <remarks>
    /// The value is optional and provides details such as operating system,
    /// CPU, GPU, RAM, and other requirements for optimal performance.
    /// </remarks>
    public string? SystemRequirements { get; set; }

    /// <summary>
    /// Represents the URL where the storage file or associated data of the game is located.
    /// Can be null if no storage URL is provided.
    /// </summary>
    public string? StorageUrl { get; set; }

    /// <summary>
    /// Gets or sets the time span representing the total amount of time the game has been played.
    /// </summary>
    public TimeSpan? TimePlayed { get; set; }
}