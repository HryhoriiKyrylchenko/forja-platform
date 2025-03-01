namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a relationship between a product and its mature content indicator.
/// </summary>
/// <remarks>
/// This entity links a product to a specific mature content descriptor, such as violence or explicit language,
/// providing additional context or restrictions for the product.
/// </remarks>
[Table("ProductMatureContents", Schema = "games")]
public class ProductMatureContent
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the identifier of the product associated with the mature content.
    /// </summary>
    /// <remarks>
    /// This property is a foreign key linking to the <see cref="Product"/> entity.
    /// </remarks>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the associated mature content entity.
    /// </summary>
    [ForeignKey("MatureContent")]
    public Guid MatureContentId { get; set; }

    /// <summary>
    /// Represents a product entity in the games domain.
    /// </summary>
    /// <remarks>
    /// A product serves as the base entity for the games domain, encapsulating details such as the title, description,
    /// developer, minimal age requirements, price, release date, and associated data like genres, discounts,
    /// and user-related collections.
    /// </remarks>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Gets or sets the mature content associated with a product.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between a product and its mature content category.
    /// It is used to indicate specific attributes of the product that may be considered mature
    /// or inappropriate for certain age groups. This property references the <see cref="MatureContent"/> entity.
    /// </remarks>
    public virtual MatureContent MatureContent { get; set; } = null!;
}