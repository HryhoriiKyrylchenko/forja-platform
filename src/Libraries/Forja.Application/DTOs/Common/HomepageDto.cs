namespace Forja.Application.DTOs.Common;

public class HomepageDto
{
    public List<GameHomeDto> Games { get; set; } = [];
    public Dictionary<string, GameHomePopularDto> PopularInGenre { get; set; } = [];
    public List<BundleDto> Bundles { get; set; } = [];
    public List<NewsArticleDto> News { get; set; } = [];
}