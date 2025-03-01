namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a mature content descriptor in the system.
/// </summary>
/// <remarks>
/// A mature content entity defines characteristics such as a name, description, and optional logo URL,
/// representing specific types of mature content (e.g., violence, explicit language). It can be associated with multiple products
/// through relationships to provide additional context or restrictions.
/// </remarks>
[Table("MatureContents", Schema = "games")]
public class MatureContent
{
    /// <summary>
    /// Represents the unique identifier for an entity.
    /// </summary>
    /// <remarks>
    /// This property is used as the primary key in the database schema to uniquely
    /// distinguish records within its respective table.
    /// </remarks>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the mature content.
    /// </summary>
    /// <remarks>
    /// This property represents the name associated with a specific mature content descriptor
    /// and is required to contain a string value with a maximum length of 50 characters.
    /// </remarks>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Provides a textual explanation or details regarding the nature of mature content.
    /// </summary>
    /// <remarks>
    /// This property allows developers or administrators to specify a detailed description
    /// for the mature content, giving users more context about its characteristics or implications.
    /// </remarks>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents the URL of the logo associated with the mature content.
    /// </summary>
    /// <remarks>
    /// This property may contain an optional string indicating the location of the logo image.
    /// Useful for visually representing the mature content in the application's UI or related materials.
    /// </remarks>
    public string? LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Navigational property representing the collection of product-mature content relationships.
    /// </summary>
    /// <remarks>
    /// This property links the mature content entity to its associated product-mature content relationships.
    /// It defines and manages the associations where specific mature content descriptors are connected to products.
    /// </remarks>
    public virtual ICollection<ProductMatureContent> ProductMatureContents { get; set; } = [];
}