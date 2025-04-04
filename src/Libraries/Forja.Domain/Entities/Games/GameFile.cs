namespace Forja.Domain.Entities.Games;

[Table("GameFiles", Schema = "games")]
public class GameFile
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("GameVersion")]
    public Guid GameVersionId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; } 
    public string Hash { get; set; } = string.Empty;
    public bool IsArchive { get; set; }
    
    public virtual GameVersion GameVersion { get; set; } = null!;
}