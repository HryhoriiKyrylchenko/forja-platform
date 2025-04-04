namespace Forja.Application.DTOs.Games;

public class GameFileDto
{
    public Guid Id { get; set; }
    public Guid GameVersionId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; } 
    public string Hash { get; set; } = string.Empty;
    public bool IsArchive { get; set; }
}