namespace Forja.Domain.Entities.Store;

[Table("Discounts", Schema = "store")]
public class Discount : SoftDeletableEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}