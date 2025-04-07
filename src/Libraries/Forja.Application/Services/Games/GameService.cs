namespace Forja.Application.Services.Games;

/// <summary>
/// Service responsible for handling business logic and data retrieval/manipulation related to games.
/// </summary>
public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IFileManagerService _fileManagerService;
        
    public GameService(IGameRepository gameRepository,
        IReviewRepository reviewRepository,
        IFileManagerService fileManagerService)
    {
        _gameRepository = gameRepository;
        _reviewRepository = reviewRepository;
        _fileManagerService = fileManagerService;
    }
     
    /// <inheritdoc />
    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        var games = await _gameRepository.GetAllAsync();
        return games.Select(GamesEntityToDtoMapper.MapToGameDto);
    }

    /// <inheritdoc />
    public async Task<List<GameCatalogDto>> GetAllForCatalogAsync()
    {
        var games = (await _gameRepository.GetAllForCatalogAsync()).ToList();
        var gameCatalogDtos = new List<GameCatalogDto>();

        foreach (var game in games)
        {
            var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(game.Id, 1900);
            var reviewCount = await _reviewRepository.GetProductApprovedReviewsCountAsync(game.Id);
    
            gameCatalogDtos.Add(GamesEntityToDtoMapper.MapToGameCatalogDto(game, logoUrl, reviewCount));
        }
        
        return gameCatalogDtos;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameDto>> GetAllDeletedAsync()
    {
        var deletedGames = await _gameRepository.GetAllDeletedAsync();
        return deletedGames.Select(GamesEntityToDtoMapper.MapToGameDto);
    }

    /// <inheritdoc />
    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var game = await _gameRepository.GetByIdAsync(id);
        return game == null ? null : GamesEntityToDtoMapper.MapToGameDto(game);
    }

    public async Task<GameExtendedDto?> GetExtendedByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        var game = await _gameRepository.GetExtendedByIdAsync(id);
        if (game == null)
            throw new KeyNotFoundException($"Game with ID {id} not found.");

        var matureContents = game.ProductMatureContents.Select(pmc => pmc.MatureContent);
        var addons = game.GameAddons;
        var mechanics = game.GameMechanics.Select(gm => gm.Mechanic);

        var matureContentDtos = new List<MatureContentDto>();
        foreach (var mc in matureContents)
        {
            matureContentDtos.Add(await GetMatureContentDtoAsync(mc));
        }

        var addonDtos = new List<GameAddonShortDto>();
        foreach (var addon in addons)
        {
            addonDtos.Add(await GetGameAddonShortDtoAsync(addon));
        }

        var mechanicDtos = new List<MechanicDto>();
        foreach (var mechanic in mechanics)
        {
            mechanicDtos.Add(await GetMechanicDtoAsync(mechanic));
        }

        var logoTask = _fileManagerService.GetPresignedProductLogoUrlAsync(game.Id, 1900);
        var productImagesTask = _fileManagerService.GetPresignedProductImagesUrlsAsync(game.Id, 1900);
        var reviewCountTask = _reviewRepository.GetProductApprovedReviewsCountAsync(game.Id);

        await Task.WhenAll(logoTask, productImagesTask, reviewCountTask);

        return GamesEntityToDtoMapper.MapToGameExtendedDto(
            game,
            logoTask.Result,
            matureContentDtos,
            productImagesTask.Result,
            addonDtos,
            mechanicDtos,
            reviewCountTask.Result
        );
    } 

    private async Task<MatureContentDto> GetMatureContentDtoAsync(MatureContent mc)
    {
        var logoUrl = mc.LogoUrl != null
            ? await _fileManagerService.GetPresignedMatureContentImageUrlAsync(mc.Id, 1900)
            : string.Empty;

        return GamesEntityToDtoMapper.MapToMatureContentDto(mc, logoUrl);
    }

    private async Task<GameAddonShortDto> GetGameAddonShortDtoAsync(GameAddon addon)
    {
        var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(addon.Id, 1900);
        return GamesEntityToDtoMapper.MapToGameAddonShortDto(addon, logoUrl);
    }

    private async Task<MechanicDto> GetMechanicDtoAsync(Mechanic mechanic)
    {
        var logoUrl = mechanic.LogoUrl != null
            ? await _fileManagerService.GetPresignedMechanicImageUrlAsync(mechanic.Id, 1900)
            : string.Empty;

        return GamesEntityToDtoMapper.MapToMechanicDto(mechanic, logoUrl);
    }

    /// <inheritdoc />
    public async Task<GameDto?> AddAsync(GameCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGamesCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var newGame = new Game
        {
            Id = Guid.NewGuid(),
            Title = request.Title, 
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Developer = request.Developer,
            MinimalAge = (MinimalAge)request.MinimalAge,
            Platforms = request.Platforms,
            Price = request.Price,
            LogoUrl = request.LogoUrl,
            ReleaseDate = request.ReleaseDate,
            IsActive = request.IsActive,
            InterfaceLanguages = request.InterfaceLanguages,
            AudioLanguages = request.AudioLanguages,
            SubtitlesLanguages = request.SubtitlesLanguages,
            SystemRequirements = request.SystemRequirements,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var addedGame = await _gameRepository.AddAsync(newGame);
        return addedGame == null ? null : GamesEntityToDtoMapper.MapToGameDto(addedGame);
    }

    /// <inheritdoc />
    public async Task<GameDto?> UpdateAsync(GameUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGamesUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingGame = await _gameRepository.GetByIdAsync(request.Id);
        if (existingGame == null)
        {
            throw new KeyNotFoundException($"Game with ID {request.Id} not found.");
        }

        existingGame.Title = request.Title; 
        existingGame.ShortDescription = request.ShortDescription;
        existingGame.Description = request.Description;
        existingGame.Developer = request.Developer;
        existingGame.MinimalAge = (MinimalAge)request.MinimalAge;
        existingGame.Platforms = request.Platforms;
        existingGame.Price = request.Price;
        existingGame.LogoUrl = request.LogoUrl;
        existingGame.ReleaseDate = request.ReleaseDate;
        existingGame.IsActive = request.IsActive;
        existingGame.InterfaceLanguages = request.InterfaceLanguages;
        existingGame.AudioLanguages = request.AudioLanguages;
        existingGame.SubtitlesLanguages = request.SubtitlesLanguages;
        existingGame.SystemRequirements = request.SystemRequirements;
        existingGame.ModifiedAt = DateTime.UtcNow;

        var updatedGame = await _gameRepository.UpdateAsync(existingGame);
        return updatedGame == null ? null : GamesEntityToDtoMapper.MapToGameDto(updatedGame);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        await _gameRepository.DeleteAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameDto>> GetByTagAsync(Guid tagId)
    {
        var games = await _gameRepository.GetByTagAsync(tagId);
        return games.Select(GamesEntityToDtoMapper.MapToGameDto);
    }
}