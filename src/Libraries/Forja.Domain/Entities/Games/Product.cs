namespace Forja.Domain.Entities.Games;

[Table("Products", Schema = "games")]
public abstract class Product : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    
    public bool IsActive { get; set; }
    
    public virtual ICollection<Discount> Discounts { get; set; } = [];
    
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
    
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];    
    
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}