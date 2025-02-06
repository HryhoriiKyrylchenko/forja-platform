namespace Forja.Domain.Entities.UserProfile;

[Table("UserLibraryAddons", Schema = "user-profile")]
public class UserLibraryAddon : SoftDeletableEntity
{
    [Key]
    [ForeignKey("UserLibraryGame")]
    public Guid UserLibraryGameId { get; set; }
    
    [Key]
    [ForeignKey("GameAddon")]
    public Guid AddonId { get; set; }
    
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    public virtual UserLibraryGame UserLibraryGame { get; set; } = null!;
    
    public virtual GameAddon GameAddon { get; set; } = null!;
}