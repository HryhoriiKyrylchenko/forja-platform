namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents the association between a product and a bundle in the system.
/// </summary>
/// <remarks>
/// This entity captures the relationship between a bundle and its products,
/// along with the distributed price of the product within the bundle.
/// </remarks>
[Table("BundleProducts", Schema = "games")]
public class BundleProduct
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle-product association.
    /// </summary>
    /// <remarks>
    /// The Id property is used as the primary key in the "BundleProducts" database table.
    /// It uniquely identifies each relationship between a bundle and a product.
    /// </remarks>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the bundle associated with a bundle product.
    /// </summary>
    /// <remarks>
    /// This property serves as a foreign key linking a bundle product to a specific bundle.
    /// It is required and cannot be empty. Validation and operations involving this identifier
    /// ensure the association between the bundle product and its corresponding bundle is maintained.
    /// </remarks>
    [ForeignKey("Bundle")]
    public Guid BundleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated product.
    /// This property is used as a foreign key to reference the product
    /// entity associated with the bundle product.
    /// </summary>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Represents the price allocated to a specific product within a bundle.
    /// </summary>
    /// <remarks>
    /// The DistributedPrice is calculated to proportionally distribute the total price of a bundle
    /// among its constituent products. It ensures that the combined DistributedPrice of all products
    /// in the bundle equals the bundle's total price. The distribution can be equal or proportional,
    /// depending on the business logic applied.
    /// </remarks>
    public decimal DistributedPrice { get; set; }

    /// <summary>
    /// Represents a collection or package of products, typically offered at a discounted price or as a single unit in a marketplace or game.
    /// </summary>
    /// <remarks>
    /// A Bundle consists of multiple products, each with its own associated price and relationship
    /// to the bundle. It includes properties to manage its state, pricing, and timeline as well as
    /// relationships to other domain objects, such as CartItems or individual products.
    /// </remarks>
    /// <example>
    /// This class is commonly used in e-commerce or gaming domains where individual products are bundled together
    /// into a unit for purchase.
    /// </example>
    public virtual Bundle Bundle { get; set; } = null!;

    /// <summary>
    /// Represents an association to a product within the context of game bundles.
    /// </summary>
    public virtual Product Product { get; set; } = null!;
}