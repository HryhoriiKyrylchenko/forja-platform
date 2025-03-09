namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service class for managing GameSave entities and encapsulating business logic.
/// </summary>
public class GameSaveService : IGameSaveService
{
    private readonly IGameSaveRepository _gameSaveRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameSaveService"/> class.
    /// </summary>
    /// <param name="gameSaveRepository">The repository for accessing GameSave data.</param>
    public GameSaveService(IGameSaveRepository gameSaveRepository)
    {
        _gameSaveRepository = gameSaveRepository ?? throw new ArgumentNullException(nameof(gameSaveRepository));
    }

    /// <inheritdoc />
    public async Task<List<GameSaveDto>> GetAllAsync()
    {
        var gameSaves = await _gameSaveRepository.GetAllAsync();
        if (gameSaves == null)
        {
            throw new InvalidOperationException("No game saves found.");
        }

        return gameSaves.Select(UserProfileEntityToDtoMapper.MapToGameSaveDto).ToList();
    }

    /// <inheritdoc />
    public async Task<GameSaveDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var gameSave = await _gameSaveRepository.GetByIdAsync(id);

        return gameSave == null ? null : UserProfileEntityToDtoMapper.MapToGameSaveDto(gameSave);
    }

    /// <inheritdoc />
    public async Task<List<GameSaveDto>> GetAllByFilterAsync(Guid? libraryGameId, Guid? userId)
    {
        var gameSaves = await _gameSaveRepository.GetAllByFilterAsync(libraryGameId, userId);
        
        return gameSaves.Select(UserProfileEntityToDtoMapper.MapToGameSaveDto).ToList();
    }

    /// <inheritdoc />
    public async Task<GameSaveDto?> AddAsync(GameSaveCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateGameSaveCreateRequest(request))
        {
            throw new ArgumentNullException(nameof(request));
        }

        var gameSave = new GameSave
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            UserId = request.UserId,
            UserLibraryGameId = request.UserLibraryGameId,
            SaveFileUrl = request.SaveFileUrl,
            CreatedAt = request.CreatedAt
        };

        var result = await _gameSaveRepository.AddAsync(gameSave);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToGameSaveDto(result);
    }

    /// <inheritdoc />
    public async Task<GameSaveDto?> UpdateAsync(GameSaveUpdateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateGameSaveUpdateRequest(request))
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var gameSave = await _gameSaveRepository.GetByIdAsync(request.Id);
        if (gameSave == null)
        {
            throw new InvalidOperationException("Game save not found.");
        }
        
        gameSave.Name = request.Name;
        gameSave.UserId = request.UserId;
        gameSave.UserLibraryGameId = request.UserLibraryGameId;
        gameSave.SaveFileUrl = request.SaveFileUrl;
        gameSave.LastUpdatedAt = request.LastUpdatedAt;

        var result = await _gameSaveRepository.UpdateAsync(gameSave);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToGameSaveDto(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        await _gameSaveRepository.DeleteAsync(id);
    }
}