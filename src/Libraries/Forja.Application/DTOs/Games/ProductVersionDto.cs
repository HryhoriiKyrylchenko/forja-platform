namespace Forja.Application.DTOs.Games;

public class ProductVersionDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public PlatformType Platform { get; set; }
    public string Version { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string Changelog { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public List<ProductFileDto> Files { get; set; } = [];
}