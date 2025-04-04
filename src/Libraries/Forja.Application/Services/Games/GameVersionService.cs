namespace Forja.Application.Services.Games;

public class GameVersionService : IGameVersionService
{
    private readonly IGameVersionRepository _gameVersionRepository;

    public GameVersionService(IGameVersionRepository gameVersionRepository)
    {
        _gameVersionRepository = gameVersionRepository;
    }
    
    public async Task<GameVersionDto?> AddGameVersionAsync(GameVersionCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameVersionCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        var gameVersion = new GameVersion
        {
            Id = Guid.NewGuid(),
            GameId = request.GameId,
            Version = request.Version,
            StorageUrl = request.StorageUrl,
            Changelog = request.Changelog,
            ReleaseDate = request.ReleaseDate
        };
        
        var result = await _gameVersionRepository.AddAsync(gameVersion);
        return result == null ? null : GamesEntityToDtoMapper.MapToGameVersionDto(result);
    }

    public async Task<GameVersionDto?> UpdateGameVersionAsync(GameVersionUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameVersionUpdateRequest(request, out var errors))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingGameVersion = await _gameVersionRepository.GetByIdAsync(request.Id);
        if (existingGameVersion == null)
        {
            throw new KeyNotFoundException($"GameVersion with ID {request.Id} not found.");
        }

        existingGameVersion.Changelog = request.ChangeLog;
        existingGameVersion.ReleaseDate = request.ReleaseDate;
        
        var result = await _gameVersionRepository.UpdateAsync(existingGameVersion);
        return result == null ? null : GamesEntityToDtoMapper.MapToGameVersionDto(result);
    }

    public async Task<GameVersionDto?> GetGameVersionByGameIdAndVersionAsync(Guid gameId, string version)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameId));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("Invalid version.", nameof(version));
        }
        
        var result = await _gameVersionRepository.GetByGameIdAndVersionAsync(gameId, version);
        return result == null ? null : GamesEntityToDtoMapper.MapToGameVersionDto(result);
    }
}