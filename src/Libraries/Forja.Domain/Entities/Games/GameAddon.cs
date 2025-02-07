namespace Forja.Domain.Entities.Games;

[Table("GameAddons", Schema = "games")]
public class GameAddon : Product
{
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    public string StorageUrl { get; set; } = string.Empty;
    
    public virtual Game Game { get; set; } = null!;
    
    public virtual ICollection<UserLibraryAddon> UserLibraryAddons { get; set; } = [];
}