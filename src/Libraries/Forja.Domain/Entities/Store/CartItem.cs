namespace Forja.Domain.Entities.Store;

[Table("CartItems", Schema = "store")]
public class CartItem : SoftDeletableEntity
{
    [Key]
    [ForeignKey("Cart")]
    public Guid CartId { get; set; }
    
    [Key]
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    public decimal Price { get; set; }
    
    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}