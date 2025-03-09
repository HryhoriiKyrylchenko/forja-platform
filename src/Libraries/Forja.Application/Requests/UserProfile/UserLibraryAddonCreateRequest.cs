namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to create an association between a library game and an add-on in the user profile system.
/// </summary>
public class UserLibraryAddonCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the game in the user's library.
    /// </summary>
    public Guid UserLibraryGameId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the addon.
    /// </summary>
    public Guid AddonId { get; set; }
}