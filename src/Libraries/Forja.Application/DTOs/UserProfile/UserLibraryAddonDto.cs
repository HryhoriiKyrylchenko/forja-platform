namespace Forja.Application.DTOs.UserProfile;

/// <summary>
/// Represents a data transfer object for user library addons, containing details
/// about a specific addon that a user has purchased or owns within their library.
/// </summary>
public class UserLibraryAddonDto
{
    /// <summary>
    /// Represents the unique identifier for the user library addon.
    /// This identifier differentiates each addon in the library and is expected to be a non-empty GUID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the game associated with the user's library.
    /// This represents a reference to the specific game to which an addon belongs.
    /// </summary>
    public Guid UserLibraryGameId { get; set; }

    /// <summary>
    /// Represents an addon for a game within the system.
    /// </summary>
    /// <remarks>
    /// The <c>GameAddon</c> class inherits from the <c>Product</c> base class and encapsulates details
    /// specific to downloadable, purchasable, or additional content that extends the main game experience.
    /// </remarks>
    public GameAddonShortDto GameAddon { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the addon was purchased by the user.
    /// </summary>
    /// <remarks>
    /// This property indicates the date of purchase for a specific addon within the user's library.
    /// The value must not represent a future date relative to the current UTC time.
    /// </remarks>
    public DateTime PurchaseDate { get; set; }
}