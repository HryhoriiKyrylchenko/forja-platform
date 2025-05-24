namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to create a new game entry in a user's library.
/// </summary>
public class UserLibraryGameCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// This property represents the ID of the user associated with the creation request.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the game to be added to the user's library.
    /// </summary>
    public Guid GameId { get; set; }
}