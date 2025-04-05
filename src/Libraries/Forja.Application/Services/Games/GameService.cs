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
        var games = await _gameRepository.GetAllForCatalogAsync();
        var gameCatalogDtos = await Task.WhenAll(games.Select(async g => 
            GamesEntityToDtoMapper.MapToGameCatalogDto(
                g,
                await _fileManagerService.GetPresignedProductLogoUrlAsync(g.Id, 1900),
                await _reviewRepository.GetProductApprovedReviewsCountAsync(g.Id)
            )));

        return gameCatalogDtos.ToList();
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
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var game = await _gameRepository.GetExtendedByIdAsync(id);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game with ID {id} not found.");
        }
        
        var productMatureContentDtos = await Task.WhenAll(
            game.ProductMatureContents.Select(async pmc =>
            {
                if (pmc.MatureContent.LogoUrl != null)
                {
                    return GamesEntityToDtoMapper.MapToMatureContentDto(
                        pmc.MatureContent,
                        await _fileManagerService.GetPresignedMatureContentImageUrlAsync(pmc.MatureContent.Id, 1900)
                    );
                }

                return GamesEntityToDtoMapper.MapToMatureContentDto(
                    pmc.MatureContent,
                    string.Empty
                );
            })
        );
        
        var productImages = await _fileManagerService.GetPresignedProductImagesUrlsAsync(game.Id, 1900);

        var gameAddonDtos = await Task.WhenAll(
            game.GameAddons.Select(async ga =>
                GamesEntityToDtoMapper.MapToGameAddonShortDto(
                    ga,
                    await _fileManagerService.GetPresignedProductLogoUrlAsync(ga.Id, 1900)
                )
            )
        );

        var gameMechanicsDtos = await Task.WhenAll(
            game.GameMechanics.Select(async gm =>
            {
                if (gm.Mechanic.LogoUrl != null)
                    return GamesEntityToDtoMapper.MapToMechanicDto(
                        gm.Mechanic,
                        await _fileManagerService.GetPresignedMechanicImageUrlAsync(gm.Mechanic.Id, 1900)
                    );
                
                return GamesEntityToDtoMapper.MapToMechanicDto(
                    gm.Mechanic,
                    String.Empty
                );
            })
        );
        
        var gameExtendedDto = GamesEntityToDtoMapper.MapToGameExtendedDto(
            game,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(game.Id, 1900),
            productMatureContentDtos.ToList(),
            productImages,
            gameAddonDtos.ToList(),
            gameMechanicsDtos.ToList(),
            await _reviewRepository.GetProductApprovedReviewsCountAsync(game.Id)
        );

        return gameExtendedDto;
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