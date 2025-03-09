namespace Forja.Application.Requests.UserProfile;

/// <summary>
/// Represents a request to update a user's library addon information.
/// </summary>
public class UserLibraryAddonUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the UserLibraryAddonUpdateRequest.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the game in the user's library to which the add-on is associated.
    /// </summary>
    public Guid UserLibraryGameId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the addon associated with the user's library update request.
    /// </summary>
    public Guid AddonId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the addon was purchased.
    /// </summary>
    public DateTime PurchaseDate { get; set; }
}