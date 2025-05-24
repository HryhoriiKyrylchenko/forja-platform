namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents an image associated with an item in the system.
/// </summary>
/// <remarks>
/// This class is part of the "games" schema and is used to store information about item images,
/// including their URL, alternative text, and associated product images.
/// </remarks>
[Table("ItemImages", Schema = "games")]
public class ItemImage
{
    /// <summary>
    /// Gets or sets the unique identifier for the item image.
    /// </summary>
    /// <remarks>
    /// This property serves as the primary key for the ItemImage entity and is used to uniquely
    /// identify an image associated with an item in the system.
    /// </remarks>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the URL of the image associated with an item.
    /// </summary>
    /// <remarks>
    /// This property contains the direct path or link to the image file,
    /// which can be displayed or utilized for representing an item visually in the system.
    /// </remarks>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative text description for the associated image.
    /// </summary>
    /// <remarks>
    /// This property is used to provide descriptive text for an image,
    /// primarily intended to improve accessibility and assist in scenarios
    /// where images cannot be displayed to users.
    /// </remarks>
    public string ImageAlt { get; set; } = string.Empty;

    /// <summary>
    /// Represents the relationship between products and their associated images in the system.
    /// </summary>
    /// <remarks>
    /// This entity is utilized to link products with their respective images within the "games" schema.
    /// It establishes a connection between product details and corresponding media resources.
    /// </remarks>
    public virtual ICollection<ProductImages> ProductImages { get; set; } = [];
}