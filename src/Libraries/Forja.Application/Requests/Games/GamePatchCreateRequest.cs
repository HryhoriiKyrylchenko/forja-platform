namespace Forja.Application.Requests.Games;

public class GamePatchCreateRequest
{
    public Guid GameId { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string FromVersion { get; set; } = string.Empty;
    public string ToVersion { get; set; } = string.Empty;
    public string PatchUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}