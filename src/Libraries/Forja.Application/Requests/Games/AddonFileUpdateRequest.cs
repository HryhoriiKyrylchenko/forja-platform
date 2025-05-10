namespace Forja.Application.Requests.Games;

public class AddonFileUpdateRequest
{
    public Guid Id { get; set; }
    public long FileSize { get; set; } 
    public string Hash { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
}