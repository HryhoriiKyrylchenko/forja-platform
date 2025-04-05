namespace Forja.Application.Services.Common;

public class HomeService : IHomeService
{
    private readonly IGameRepository _gameRepository;
    private readonly IBundleRepository _bundleRepository;
    private readonly INewsArticleRepository _newsArticleRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IFileManagerService _fileManagerService;
    
    public HomeService(IGameRepository gameRepository,
        IBundleRepository bundleRepository,
        INewsArticleRepository newsArticleRepository,
        IReviewRepository reviewRepository,
        IFileManagerService fileManagerService)
    {
        _gameRepository = gameRepository;
        _bundleRepository = bundleRepository;
        _newsArticleRepository = newsArticleRepository;
        _reviewRepository = reviewRepository;
        _fileManagerService = fileManagerService;
    }
    
    public async Task<HomepageDto> GetHomepageDataAsync()
    {
        var allGames = await _gameRepository.GetAllAsync();
        var allBundles = await _bundleRepository.GetAllActiveAsync();
        var allNewsArticles = await _newsArticleRepository.GetActiveNewsArticlesAsync();
        
        var gameWithReviews = await Task.WhenAll(allGames.Select(async game =>
        {
            var (positive, negative) = await _reviewRepository.GetProductApprovedReviewsCountAsync(game.Id);
            return new
            {
                Game = game,
                PositiveReviews = positive,
                NegativeReviews = negative
            };
        }));
        
        var top5Games = gameWithReviews
            .OrderByDescending(g => g.PositiveReviews)
            .ThenBy(g => g.NegativeReviews) 
            .Take(5)
            .Select(g => g.Game)
            .ToList();
        
        var bestGamesByGenre = await Task.WhenAll(
            gameWithReviews
                .SelectMany(g => g.Game.ProductGenres.Select(pg => new
                {
                    GenreName = pg.Genre.Name,
                    g.Game,
                    g.PositiveReviews,
                    g.NegativeReviews
                }))
                .GroupBy(g => g.GenreName)
                .Select(async g =>
                {
                    var topGame = g
                        .OrderByDescending(x => x.PositiveReviews)
                        .ThenBy(x => x.NegativeReviews)
                        .First();

                    var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(topGame.Game.Id);

                    return new
                    {
                        Genre = topGame.GenreName,
                        Game = new GameHomePopularDto
                        {
                            Id = topGame.Game.Id,
                            Title = topGame.Game.Title,
                            LogoUrl = logoUrl
                        }
                    };
                })
        );
        
        var bestGamesByGenreDict = bestGamesByGenre
            .ToDictionary(x => x.Genre, x => x.Game);
        
        var topGames = await Task.WhenAll(top5Games.Select(async g =>
        {
            var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(g.Id);
            var images = await _fileManagerService.GetPresignedProductImagesUrlsAsync(g.Id);
            return GamesEntityToDtoMapper.MapToGameHomeDto(g, logoUrl, images);
        }));

        var bundles = await Task.WhenAll(allBundles.Select(async b =>
        {
            var bundleProductDtos = await Task.WhenAll(b.BundleProducts.Select(async bp =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.ProductId);
                return GamesEntityToDtoMapper.MapToBundleProductDto(bp, logoUrl);
            }));

            return GamesEntityToDtoMapper.MapToBundleDto(b, bundleProductDtos.ToList());
        }));
        
        var newsArticles = await Task.WhenAll(allNewsArticles.Select(async na => CommonEntityToDtoMapper.MapToNewsArticleDto(
            na,
            na.ImageUrl == null ? string.Empty
                : await _fileManagerService.GetPresignedUrlAsync(na.ImageUrl)
        )));

        return new HomepageDto
        {
            Games = topGames.ToList(),
            PopularInGenre = bestGamesByGenreDict,
            Bundles = bundles.ToList(),
            News = newsArticles.ToList()
        };
    }
}