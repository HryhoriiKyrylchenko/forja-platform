namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents a discount entity that can be applied to products in a store.
/// </summary>
[Table("Discounts", Schema = "store")]
public class Discount : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for an object or entity.
    /// </summary>
    /// <remarks>
    /// The Id property is typically used to uniquely distinguish an instance of a class.
    /// It is often implemented as a primary key in data storage or databases.
    /// </remarks>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the discount.
    /// </summary>
    /// <remarks>
    /// The name is a required field with a maximum length of 50 characters.
    /// </remarks>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the type of discount applied to a product or service.
    /// </summary>
    /// <remarks>
    /// The <see cref="DiscountType"/> enumeration defines the method by which a discount is calculated.
    /// It can be one of the following values:
    /// - Percentage: Represents a discount applied as a percentage of the original price.
    /// - Fixed: Represents a discount applied as a fixed monetary value.
    /// </remarks>
    [Required]
    public DiscountType DiscountType { get; set; }

    /// <summary>
    /// Represents the value of the discount applicable in a specified context.
    /// The value can be interpreted either as a percentage or as a flat amount,
    /// depending on the <see cref="DiscountType"/> configuration.
    /// </summary>
    /// <remarks>
    /// Must be a positive number greater than zero. Validation ensures its correctness.
    /// </remarks>
    public decimal DiscountValue { get; set; }

    /// <summary>
    /// Represents the start date of the discount.
    /// This property specifies the date and time from which the discount becomes active.
    /// It is nullable, allowing discounts to have no specific start date, in which case the discount is considered active indefinitely until its end date, if any.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// Represents the optional end date of the discount period.
    /// If the value is null, the discount does not have a specified end date.
    /// Used to determine the duration of the discount and validate its applicability.
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Navigation property that represents the collection of product discounts associated with the discount.
    /// Links multiple products to a specific discount.
    /// </summary>
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}