namespace Forja.Domain.Entities.UserProfile;

[Table("UserLibraryAddons", Schema = "user-profile")]
public class UserLibraryAddon : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [ForeignKey("UserLibraryGame")]
    public Guid UserLibraryGameId { get; set; }

    [ForeignKey("GameAddon")]
    public Guid AddonId { get; set; }
    
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    public virtual UserLibraryGame UserLibraryGame { get; set; } = null!;
    
    public virtual GameAddon GameAddon { get; set; } = null!;
}