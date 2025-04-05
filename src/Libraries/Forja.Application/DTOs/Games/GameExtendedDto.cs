namespace Forja.Application.DTOs.Games;

public class GameExtendedDto
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
    public string InterfaceLanguages { get; set; } = string.Empty;
    public string AudioLanguages { get; set; } = string.Empty;
    public string SubtitlesLanguages { get; set; } = string.Empty;
    public string? SystemRequirements { get; set; }
    public List<string> Images { get; set; } = [];
    public List<GameAddonShortDto> Addons { get; set; } = [];
    public List<DiscountDto> Discounts { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
    public List<TagDto> Tags { get; set; } = [];
    public List<MechanicDto> Mechanics { get; set; } = [];
    public List<MatureContentDto> MatureContent { get; set; } = [];
    public (int positive, int negative) Rating { get; set; }
}