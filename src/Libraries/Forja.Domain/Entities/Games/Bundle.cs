namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a bundle of products in the games domain.
/// </summary>
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
    [Required]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the bundle.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total price of the bundle.
    /// </summary>
    public decimal TotalPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the bundle was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets a value indicating whether the bundle is active.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of products included in the bundle.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<BundleProduct> BundleProducts { get; set; } = [];
}