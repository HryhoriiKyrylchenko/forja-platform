namespace Forja.Application.DTOs.UserProfile;

/// <summary>
/// Represents a data transfer object (DTO) for a user's library game.
/// </summary>
public class UserLibraryGameDto
{
    /// <summary>
    /// Represents the unique identifier for the UserLibraryGame entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the game in the user's library.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents a game entity within the application.
    /// </summary>
    /// <remarks>
    /// This class extends the <see cref="Product"/> class to inherit common product properties.
    /// It includes additional attributes specific to games, such as system requirements and storage URL.
    /// </remarks>
    /// <property name="SystemRequirements">
    /// Gets or sets the detailed system requirements necessary to run the game.
    /// </property>
    /// <property name="StorageUrl">
    /// Gets or sets the storage location URL where the game data or assets are hosted.
    /// </property>
    public GameDto Game { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the total time played for the game.
    /// </summary>
    public TimeSpan? TimePlayed { get; set; }

    /// <summary>
    /// Represents the date and time when the game was purchased by the user.
    /// </summary>
    /// <remarks>
    /// This property should always store a valid date and time value that is not in the future.
    /// </remarks>
    public DateTime PurchaseDate { get; set; }
}