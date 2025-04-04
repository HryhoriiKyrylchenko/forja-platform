namespace Forja.Application.Requests.Games;

public class GameFileCreateRequest
{
    public Guid GameVersionId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; } 
    public string Hash { get; set; } = string.Empty;
}