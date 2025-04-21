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
    
    ///<inheritdoc/>
    public async Task<HomepageDto> GetHomepageDataAsync()
    {
        var allGames = await _gameRepository.GetAllAsync();
        var allBundles = await _bundleRepository.GetAllActiveAsync();
        var allNewsArticles = await _newsArticleRepository.GetActiveNewsArticlesAsync();

        var gameWithReviews = new List<(Game Game, int PositiveReviews, int NegativeReviews)>();
        foreach (var game in allGames)
        {
            var (positive, negative) = await _reviewRepository.GetProductApprovedReviewsCountAsync(game.Id);
            gameWithReviews.Add((game, positive, negative));
        }

        var top5Games = gameWithReviews
            .OrderByDescending(g => g.PositiveReviews)
            .ThenBy(g => g.NegativeReviews)
            .Take(5)
            .Select(g => g.Game)
            .ToList();

        var bestGamesByGenre = new Dictionary<string, GameHomePopularDto>();
        var gamesGroupedByGenre = gameWithReviews
            .SelectMany(g => g.Game.ProductGenres.Select(pg => new
            {
                GenreName = pg.Genre.Name,
                Game = g.Game,
                PositiveReviews = g.PositiveReviews,
                NegativeReviews = g.NegativeReviews
            }))
            .GroupBy(x => x.GenreName);

        foreach (var genreGroup in gamesGroupedByGenre)
        {
            var topGame = genreGroup
                .OrderByDescending(x => x.PositiveReviews)
                .ThenBy(x => x.NegativeReviews)
                .First();

            var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(topGame.Game.Id);

            bestGamesByGenre.Add(genreGroup.Key, new GameHomePopularDto
            {
                Id = topGame.Game.Id,
                Title = topGame.Game.Title,
                LogoUrl = logoUrl
            });
        }

        var topGames = new List<GameHomeDto>();
        foreach (var game in top5Games)
        {
            var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(game.Id);
            var images = await _fileManagerService.GetPresignedProductImagesUrlsAsync(game.Id);
            topGames.Add(GamesEntityToDtoMapper.MapToGameHomeDto(game, logoUrl, images));
        }

        var bundles = new List<BundleDto>();
        foreach (var bundle in allBundles)
        {
            var bundleProductDtos = new List<BundleProductDto>();
            foreach (var bp in bundle.BundleProducts)
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.ProductId);
                bundleProductDtos.Add(GamesEntityToDtoMapper.MapToBundleProductDto(bp, logoUrl));
            }
            bundles.Add(GamesEntityToDtoMapper.MapToBundleDto(bundle, bundleProductDtos));
        }

        var newsArticles = new List<NewsArticleDto>();
        foreach (var article in allNewsArticles)
        {
            var imageUrl = article.ImageUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(article.ImageUrl)
                : string.Empty;

            newsArticles.Add(CommonEntityToDtoMapper.MapToNewsArticleDto(article, imageUrl));
        }

        return new HomepageDto
        {
            Games = topGames,
            PopularInGenre = bestGamesByGenre,
            Bundles = bundles,
            News = newsArticles
        };
    }
}