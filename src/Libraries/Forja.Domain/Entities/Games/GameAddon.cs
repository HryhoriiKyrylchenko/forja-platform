namespace Forja.Domain.Entities.Games;

[Table("GameAddons", Schema = "games")]
public class GameAddon : Product
{
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    public string StorageUrl { get; set; } = string.Empty;
    
    public virtual Game Game { get; set; } = null!;
    
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
    
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
    
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
    
    public virtual ICollection<UserLibraryAddon> UserLibraryAddons { get; set; } = [];
    
    public virtual ICollection<Discount> Discounts { get; set; } = [];
}