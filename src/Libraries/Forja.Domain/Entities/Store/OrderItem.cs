namespace Forja.Domain.Entities.Store;

[Table("OrderItems", Schema = "store")]
public class OrderItem : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }

    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    public decimal FinalPrice { get; set; }
    
    public virtual Order Order { get; set; } = null!;
    
    public virtual Product Product { get; set; } = null!;
}