namespace Forja.Domain.Entities.Store;

[Table("Discounts", Schema = "store")]
public class Discount : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DiscountType DiscountType { get; set; }

    // For Percentage type, use values between 0 and 100; for Fixed type, it is the discount amount.
    public decimal DiscountValue { get; set; }

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}