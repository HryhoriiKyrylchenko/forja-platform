namespace Forja.Application.DTOs.Games;

public class GameWishListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public List<DiscountDto> Discounts { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
    public List<AchievementShortDto> Achievements { get; set; } = [];
    public List<GameAddonSmallDto> Addons { get; set; } = [];
    public int PositiveRating { get; set; }
    public int NegativeRating { get; set; }
}