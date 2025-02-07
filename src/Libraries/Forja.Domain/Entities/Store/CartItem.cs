namespace Forja.Domain.Entities.Store;

[Table("CartItems", Schema = "store")]
public class CartItem : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [ForeignKey("Cart")]
    public Guid CartId { get; set; }
    
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    public decimal Price { get; set; }
    
    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}