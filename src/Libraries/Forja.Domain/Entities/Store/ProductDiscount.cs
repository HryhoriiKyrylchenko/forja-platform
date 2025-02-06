namespace Forja.Domain.Entities.Store;

[Table("ProductDiscounts", Schema = "store")]
public class ProductDiscount
{
    [Key]
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    [Key]
    [ForeignKey("Discount")]
    public Guid DiscountId { get; set; }
    
    public virtual Product Product { get; set; } = null!;
    
    public virtual Discount Discount { get; set; } = null!;
}