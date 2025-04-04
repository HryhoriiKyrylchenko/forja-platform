namespace Forja.Application.Requests.Games;

public class GameFileUpdateRequest
{
    public Guid Id { get; set; }
    public long FileSize { get; set; } 
    public string Hash { get; set; } = string.Empty;
}