namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a product that is part of a bundle in the games domain.
/// </summary>
[Table("BundleProducts", Schema = "games")]
public class BundleProduct
{
    /// <summary>
    /// Gets or sets the unique identifier for the bundle product.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the associated bundle.
    /// </summary>
    [ForeignKey("Bundle")]
    public Guid BundleId { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the associated product.
    /// </summary>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the bundle associated with this bundle product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Bundle Bundle { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the product associated with this bundle product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Product Product { get; set; } = null!;
}