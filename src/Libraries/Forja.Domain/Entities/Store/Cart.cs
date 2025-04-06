namespace Forja.Domain.Entities.Store;

[Table("Carts", Schema = "store")]
public class Cart
{
    [Key]
    public Guid Id { get; set; } 
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public CartStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
}