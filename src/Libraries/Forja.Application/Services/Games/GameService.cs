namespace Forja.Application.Services.Games;

/// <summary>
/// Service responsible for handling business logic and data retrieval/manipulation related to games.
/// </summary>
public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
        
    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
     
    /// <inheritdoc />
    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        var games = await _gameRepository.GetAllAsync();
        return games.Select(GamesEntityToDtoMapper.MapToGameDto);
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

    /// <inheritdoc />
    public async Task<GameDto?> GetByStorageUrlAsync(string storageUrl)
    {
        if (string.IsNullOrWhiteSpace(storageUrl))
        {
            throw new ArgumentException("Storage URL cannot be empty.", nameof(storageUrl));
        }
        
        var game = await _gameRepository.GetByStorageUrlAsync(storageUrl);
        return game == null ? null : GamesEntityToDtoMapper.MapToGameDto(game);
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
            StorageUrl = request.StorageUrl,
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
        existingGame.StorageUrl = request.StorageUrl;
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