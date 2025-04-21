namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a bundle of products, intended to group multiple items together,
/// typically offered at a combined price. Bundles are used in scenarios such as
/// product packages or promotional offers.
/// </summary>
/// <remarks>
/// This class is part of the "games" schema in the database. It includes metadata
/// such as a unique identifier, descriptive information, pricing details, and
/// timestamps for creation and expiration. A bundle can include multiple products
/// and can be associated with shopping cart items.
/// </remarks>
[Table("Bundles", Schema = "games")]
public class Bundle
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the bundle.
    /// </summary>
    /// <remarks>
    /// The title is a required string property with a maximum length of 200 characters.
    /// It represents the name or label of the bundle.
    /// </remarks>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// A brief textual representation providing additional details about the bundle.
    /// </summary>
    /// <remarks>
    /// The description offers a more elaborate understanding or context about the bundle,
    /// typically summarizing its purpose or contents. It can be left empty by default
    /// but must adhere to length restrictions as defined in validation rules.
    /// </remarks>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents the total price of the bundle, including all associated products and items.
    /// </summary>
    /// <remarks>
    /// The value of this property must be non-negative. It is used in various operations, such as bundle creation,
    /// validation, and price comparison to ensure consistency across application workflows.
    /// </remarks>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Represents the creation date and time for a bundle.
    /// </summary>
    /// <remarks>
    /// This property is automatically set to the current UTC date and time when a new bundle is created.
    /// It is used to track when the bundle was initially added to the system.
    /// </remarks>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the bundle expires.
    /// This property determines the validity period of the bundle.
    /// If the value is null, the bundle does not have an expiration date.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Represents the activation state of the bundle.
    /// </summary>
    /// <remarks>
    /// A value indicating whether the bundle is active or not. An active bundle is typically considered valid and available for use
    /// in operations such as purchase or association with other entities.
    /// </remarks>
    public bool IsActive { get; set; }

    /// <summary>
    /// Represents the collection of products associated with a specific bundle.
    /// </summary>
    /// <remarks>
    /// This property establishes the relationship between a bundle and its respective products,
    /// enabling navigation and access to the details of the products included in the bundle.
    /// It is a virtual property facilitating Entity Framework lazy or explicit loading.
    /// </remarks>
    public virtual ICollection<BundleProduct> BundleProducts { get; set; } = [];

    /// <summary>
    /// Represents the collection of cart items associated with a bundle.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between a bundle and the cart items
    /// that include this bundle. It is used to track which cart items contain the bundle
    /// in the application.
    /// </remarks>
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
}