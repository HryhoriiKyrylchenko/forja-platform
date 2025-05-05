namespace Forja.Application.DTOs.Games;

public class GameAddonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public MinimalAge MinimalAge { get; set; }
    public string Platforms { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string LogoUrl { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public bool IsActive { get; set; }
    public string InterfaceLanguages { get; set; } = string.Empty;
    public string AudioLanguages { get; set; } = string.Empty;
    public string SubtitlesLanguages { get; set; } = string.Empty;
    public Guid GameId { get; set; }
    public List<DiscountDto> Discounts { get; set; } = [];
    public List<MatureContentDto> MatureContent { get; set; } = [];
    public (int positive, int negative) Rating { get; set; }
}