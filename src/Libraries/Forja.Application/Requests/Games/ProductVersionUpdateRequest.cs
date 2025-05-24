namespace Forja.Application.Requests.Games;

public class ProductVersionUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string ChangeLog { get; set; } = string.Empty;
    [Required]
    public DateTime ReleaseDate { get; set; }
}