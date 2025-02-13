namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents a discount applied to a product in the store.
/// </summary>
[Table("ProductDiscounts", Schema = "store")]
public class ProductDiscount
{
    /// <summary>
    /// Gets or sets the unique identifier for the product discount.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the discount.
    /// </summary>
    [ForeignKey("Discount")]
    public Guid DiscountId { get; set; }
    
    /// <summary>
    /// Gets or sets the product associated with this discount.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Product Product { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the discount associated with this product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Discount Discount { get; set; } = null!;
}