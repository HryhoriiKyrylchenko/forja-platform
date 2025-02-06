namespace Forja.Domain.Entities.Games;

[Table("Games", Schema = "games")]
public class Game : Product
{
    [Required]
    public DateTime ReleaseDate { get; set; }
    
    public string SystemRequirements { get; set; } = string.Empty;
    
    public string StorageUrl { get; set; } = string.Empty;
    
    public virtual ICollection<GameCategory> GameCategories { get; set; } = [];
    
    public virtual ICollection<GameAddon> GameAddons { get; set; } = [];
    
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
    
    public virtual ICollection<GameTag> GameTags { get; set; } = [];
    
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
    
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
    
    public virtual ICollection<UserLibraryGame> UserLibraryGames { get; set; } = [];
    
    public virtual ICollection<Review> Reviews { get; set; } = [];
    
    public virtual ICollection<Discount> Discounts { get; set; } = [];
}