namespace Forja.Domain.Entities.Games;

[Table("GamePatches", Schema = "games")]
public class GamePatch
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Game")]
    public Guid GameId { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string FromVersion { get; set; } = string.Empty;
    public string ToVersion { get; set; } = string.Empty;
    public string PatchUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    
    public virtual Game Game { get; set; } = null!;
}