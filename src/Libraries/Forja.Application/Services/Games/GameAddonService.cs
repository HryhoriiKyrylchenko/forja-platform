namespace Forja.Application.Services.Games;

/// <summary>
/// Service class responsible for handling game addon operations.
/// </summary>
public class GameAddonService : IGameAddonService
{
    private readonly IGameAddonRepository _gameAddonRepository;

    public GameAddonService(IGameAddonRepository gameAddonRepository)
    {
        _gameAddonRepository = gameAddonRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameAddonDto>> GetAllAsync()
    {
        var addons = await _gameAddonRepository.GetAllAsync();
        return addons.Select(GamesEntityToDtoMapper.MapToGameAddonDto);
    }

    /// <inheritdoc />
    public async Task<GameAddonDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var addon = await _gameAddonRepository.GetByIdAsync(id);
        return addon == null ? null : GamesEntityToDtoMapper.MapToGameAddonDto(addon);
    }

    /// <inheritdoc />
    public async Task<GameAddonDto?> CreateAsync(GameAddonCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameAddonCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var newAddon = new GameAddon
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
            GameId = request.GameId,
            StorageUrl = request.StorageUrl,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var createdAddon = await _gameAddonRepository.AddAsync(newAddon);
        return createdAddon == null ? null : GamesEntityToDtoMapper.MapToGameAddonDto(createdAddon);
    }

    /// <inheritdoc />
    public async Task<GameAddonDto?> UpdateAsync(GameAddonUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameAddonUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingAddon = await _gameAddonRepository.GetByIdAsync(request.Id);
        if (existingAddon == null)
        {
            throw new KeyNotFoundException($"GameAddon with ID {request.Id} not found.");
        }

        existingAddon.Title = request.Title;
        existingAddon.ShortDescription = request.ShortDescription;
        existingAddon.Description = request.Description;
        existingAddon.Developer = request.Developer;
        existingAddon.MinimalAge = (MinimalAge)request.MinimalAge;
        existingAddon.Platforms = request.Platforms;
        existingAddon.Price = request.Price;
        existingAddon.LogoUrl = request.LogoUrl;
        existingAddon.ReleaseDate = request.ReleaseDate;
        existingAddon.IsActive = request.IsActive;
        existingAddon.InterfaceLanguages = request.InterfaceLanguages;
        existingAddon.AudioLanguages = request.AudioLanguages;
        existingAddon.SubtitlesLanguages = request.SubtitlesLanguages;
        existingAddon.GameId = request.GameId;
        existingAddon.StorageUrl = request.StorageUrl;
        existingAddon.ModifiedAt = DateTime.UtcNow;

        var updatedAddon = await _gameAddonRepository.UpdateAsync(existingAddon);
        return updatedAddon == null ? null : GamesEntityToDtoMapper.MapToGameAddonDto(updatedAddon);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        await _gameAddonRepository.DeleteAsync(id);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<GameAddonDto>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameId));
        }
        
        var addons = await _gameAddonRepository.GetByGameIdAsync(gameId);
        return addons.Select(GamesEntityToDtoMapper.MapToGameAddonDto);
    }
}