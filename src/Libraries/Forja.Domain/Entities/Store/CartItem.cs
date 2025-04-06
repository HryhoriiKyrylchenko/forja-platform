namespace Forja.Domain.Entities.Store;

[Table("CartItems", Schema = "store")]
public class CartItem
{
    [Key]
    public Guid Id { get; set; }
    [ForeignKey("Cart")]
    public Guid CartId { get; set; }
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    public Guid? BundleId { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be at least 0.01")]
    public decimal Price { get; set; }
    public virtual Cart Cart { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual Bundle? Bundle { get; set; }
}