namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents an association between a product and an item image within the "games" schema.
/// </summary>
/// <remarks>
/// This class establishes a many-to-many relationship between the <see cref="Product"/> and
/// <see cref="ItemImage"/> entities. It maps product images to the corresponding products
/// and item images stored in the database.
/// </remarks>
[Table("ProductImages", Schema = "games")]
public class ProductImages
{
    /// <summary>
    /// Gets or sets the unique identifier for the ProductImages entity.
    /// </summary>
    /// <remarks>
    /// This property serves as the primary key for the ProductImages table within the "games" schema.
    /// It uniquely identifies a relationship between a product and its corresponding image.
    /// </remarks>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the associated product.
    /// </summary>
    /// <remarks>
    /// This property establishes a foreign key relationship with the <see cref="Product"/> entity
    /// and is used to connect product images to their corresponding product in the "games" schema.
    /// </remarks>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the associated item image in the ProductImages table.
    /// </summary>
    /// <remarks>
    /// This property serves as a foreign key linking the ProductImages entity to the ItemImage entity,
    /// establishing the relationship between a product's image and its details in the system.
    /// </remarks>
    [ForeignKey("ItemImage")]
    public Guid ItemImageId { get; set; }

    /// <summary>
    /// Represents a product in the games domain that inherits from a soft deletable entity.
    /// </summary>
    /// <remarks>
    /// The product entity contains essential information such as the title, description, developer name, minimal age rating, pricing, logo URL, dates for creation, modification, and release, as well as its active status.
    /// It also manages various relationships with other entities, such as genres, discounts, cart items, order items, mature content, and product images.
    /// </remarks>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Represents the image associated with a product.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between the <see cref="ProductImages"/> and <see cref="ItemImage"/> entities.
    /// It defines the specific image details through the associated <see cref="ItemImage"/> entity.
    /// </remarks>
    public virtual ItemImage ItemImage { get; set; } = null!;
}