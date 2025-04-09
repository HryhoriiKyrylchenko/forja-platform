namespace Forja.Application.DTOs.Games;

public class GameCatalogDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public List<DiscountDto> Discounts { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
    public List<TagDto> Tags { get; set; } = [];
    public List<MechanicDto> Mechanics { get; set; } = [];
    public List<MatureContentDto> MatureContents { get; set; } = [];
    public int PositiveRating { get; set; }
    public int NegativeRating { get; set; }
}