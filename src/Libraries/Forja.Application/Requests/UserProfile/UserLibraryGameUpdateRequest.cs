namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to update a user's game information in their library.
/// </summary>
public class UserLibraryGameUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier associated with the user library game update request.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the user associated with the library game update request.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the game associated with the user library update request.
    /// </summary>
    public Guid GameId { get; set; }
    
    /// <summary>
    /// Gets or sets the total time played for the game.
    /// </summary>
    public TimeSpan? TimePlayed { get; set; }

    /// <summary>
    /// Represents the date and time when the game was purchased.
    /// This property is part of a user's library game update request.
    /// </summary>
    public DateTime PurchaseDate { get; set; }
}