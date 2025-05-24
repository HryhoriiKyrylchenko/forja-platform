namespace Forja.Application.Services.Games;

public class GamePatchService : IGamePatchService
{
    private readonly IGamePatchRepository _gamePatchRepository;

    public GamePatchService(IGamePatchRepository gamePatchRepository)
    {
        _gamePatchRepository = gamePatchRepository;
    }
    
    public async Task<GamePatchDto?> AddGamePatch(GamePatchCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGamePatchCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Request validation failed. Error: {errors}");
        }

        var gamePatch = new GamePatch
        {
            Id = Guid.NewGuid(),
            GameId = request.GameId,
            Platform = request.Platform,
            Name = request.Name,
            FromVersion = request.FromVersion,
            ToVersion = request.ToVersion,
            PatchUrl = request.PatchUrl,
            FileSize = request.FileSize,
            Hash = request.Hash,
            ReleaseDate = request.ReleaseDate
        };
        
        var result = await _gamePatchRepository.AddAsync(gamePatch);
        return result == null ? null : GamesEntityToDtoMapper.MapToGamePatchDto(result);
    }

    public async Task<GamePatchDto?> UpdateGamePatch(GamePatchUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGamePatchUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Request validation failed. Error: {errors}");
        }
        
        var existingGamePatch = await _gamePatchRepository.GetByIdAsync(request.Id);
        if (existingGamePatch == null)
        {
            throw new KeyNotFoundException($"GamePatch with ID {request.Id} not found.");
        }
        
        existingGamePatch.Name = request.Name;
        existingGamePatch.FromVersion = request.FromVersion;
        existingGamePatch.ToVersion = request.ToVersion;
        existingGamePatch.FileSize = request.FileSize;
        existingGamePatch.Hash = request.Hash;
        existingGamePatch.ReleaseDate = request.ReleaseDate;
        
        var result = await _gamePatchRepository.UpdateAsync(existingGamePatch);
        return result == null ? null : GamesEntityToDtoMapper.MapToGamePatchDto(result);
    }

    public async Task<GamePatchDto?> GetGamePatchByGameIdPlatformAndName(Guid gameId, PlatformType platform, string name)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Invalid name.", nameof(name));
        }
        
        var result = await _gamePatchRepository.GetByGameIdPlatformAndPatchNameAsync(gameId, platform, name);
        return result == null ? null : GamesEntityToDtoMapper.MapToGamePatchDto(result);
    }
}