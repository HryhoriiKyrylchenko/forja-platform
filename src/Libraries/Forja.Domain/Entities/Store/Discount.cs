namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents a discount entity in the store.
/// </summary>
[Table("Discounts", Schema = "store")]
public class Discount : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the discount.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the discount.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the discount.
    /// </summary>
    [Required]
    public DiscountType DiscountType { get; set; }

    /// <summary>
    /// Gets or sets the value of the discount.
    /// For Percentage type, use values between 0 and 100; for Fixed type, it is the discount amount.
    /// </summary>
    public decimal DiscountValue { get; set; }

    /// <summary>
    /// Gets or sets the start date of the discount.
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Gets or sets the end date of the discount.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the collection of product discounts associated with this discount.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}