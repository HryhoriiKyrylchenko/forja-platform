namespace Forja.Domain.Entities.Store;

[Table("OrderItems", Schema = "store")]
public class OrderItem : SoftDeletableEntity
{
    [Key]
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }
    
    [Key]
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    public decimal FinalPrice { get; set; }
    
    public virtual Order Order { get; set; } = null!;
    
    public virtual Product Product { get; set; } = null!;
}