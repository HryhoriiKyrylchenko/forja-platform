namespace Forja.Domain.Entities.UserProfile;

[Table("UserLibraryGames", Schema = "user-profile")]
public class UserLibraryGame : SoftDeletableEntity
{
    [Key]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Key]
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
    
    public virtual Game Game { get; set; } = null!;
    
    public virtual ICollection<UserLibraryAddon> PurchasedAddons { get; set; } = [];
}