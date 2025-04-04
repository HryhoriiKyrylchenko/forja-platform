namespace Forja.Domain.Entities.Games;

[Table("GameVersions", Schema = "games")]
public class GameVersion
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    [MaxLength(15)]
    public string Version { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string Changelog { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }

    public virtual Game Game { get; set; } = null!;
    public virtual ICollection<GameFile> Files { get; set; } = [];
}