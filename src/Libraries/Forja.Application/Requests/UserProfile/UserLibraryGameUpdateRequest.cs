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
    /// Gets or sets the total time played for the game.
    /// </summary>
    public TimeSpan TimePlayed { get; set; }
}