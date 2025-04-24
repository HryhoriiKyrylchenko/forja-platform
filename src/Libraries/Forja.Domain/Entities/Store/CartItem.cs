namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents an item in a shopping cart. Each cart item is associated with a specific product and optionally belongs to a bundle.
/// It contains information such as its unique identifier, the associated cart, product, bundle, and its price.
/// </summary>
[Table("CartItems", Schema = "store")]
public class CartItem
{
    /// <summary>
    /// Gets or sets the unique identifier for the cart item.
    /// </summary>
    /// <remarks>
    /// This property serves as the primary key for the CartItem entity.
    /// </remarks>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier for a cart associated with a CartItem.
    /// This property serves as a foreign key linking the CartItem to its corresponding Cart entity.
    /// </summary>
    [ForeignKey("Cart")]
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product associated with the cart item.
    /// </summary>
    /// <remarks>
    /// This property is a foreign key linking the cart item to its corresponding product in the "Products" table.
    /// It is used in various operations, such as calculating discounts, processing orders, and managing user library items.
    /// </remarks>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the associated bundle in the cart item.
    /// A null value indicates that the cart item is not part of any bundle.
    /// </summary>
    public Guid? BundleId { get; set; }

    /// <summary>
    /// Represents the price of a cart item in the store.
    /// This property is a decimal value with a precision of 18 and scale of 2.
    /// </summary>
    /// <remarks>
    /// The price is required to be at least 0.01 and is validated to ensure it does not go below this minimum value.
    /// Ensures accurate calculation and validation of item and bundle pricing in the context of shopping cart operations.
    /// </remarks>
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be at least 0.01")]
    public decimal Price { get; set; }

    /// <summary>
    /// Represents a shopping cart entity within the store domain.
    /// </summary>
    /// <remarks>
    /// A Cart is associated with a specific user and contains a collection of cart items.
    /// It includes details like the total amount of the cart, its status, and timestamps
    /// for when it was created or last modified.
    /// </remarks>
    public virtual Cart Cart { get; set; } = null!;

    /// <summary>
    /// Represents a product within the system, providing details about the product's attributes
    /// such as title, description, developer, pricing, release date, platform compatibility,
    /// languages supported, and more.
    /// </summary>
    /// <remarks>
    /// The <see cref="Product"/> entity is abstract and intended to be used as a base class
    /// for defining specific types of products. It contains essential metadata about the product
    /// and supports relationships with other entities such as genres, images, reviews, discounts,
    /// and user interactions (e.g., wish lists, cart items).
    /// </remarks>
    /// <example>
    /// Members of this class include properties for storing information like price,
    /// release date, multimedia languages, and relationships to other entities
    /// (e.g., <see cref="CartItem"/> and <see cref="ProductDiscount"/>).
    /// </example>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Represents a collection or grouping of multiple products within a bundle entity.
    /// This property links a cart item to a specific bundle, describing the association
    /// between the cart's contents and promotional or packaged product sets.
    /// </summary>
    /// <remarks>
    /// A bundle usually encompasses one or more products designed to be sold together,
    /// either for convenience or to take advantage of discounts. This property can be null,
    /// indicating that the cart item is not associated with any bundle.
    /// </remarks>
    public virtual Bundle? Bundle { get; set; }
}