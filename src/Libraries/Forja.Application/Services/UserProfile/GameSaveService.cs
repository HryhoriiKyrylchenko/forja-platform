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
    public async Task<IEnumerable<GameSaveDto>> GetAllAsync()
    {
        var gameSaves = await _gameSaveRepository.GetAllAsync();
        if (gameSaves == null)
        {
            throw new InvalidOperationException("No game saves found.");
        }

        return gameSaves.Select(gs => new GameSaveDto
        {
            Id = gs.Id,
            SaveFileUrl = gs.SaveFileUrl,
            CreatedAt = gs.CreatedAt,
            LastUpdatedAt = gs.LastUpdatedAt,
            UserId = gs.UserId,
            UserLibraryGameId = gs.UserLibraryGameId
        });
    }

    /// <inheritdoc />
    public async Task<GameSaveDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var gameSave = await _gameSaveRepository.GetByIdAsync(id);

        if (gameSave == null)
        {
            throw new InvalidOperationException("Game save not found.");
        }

        return new GameSaveDto
        {
            Id = gameSave.Id,
            SaveFileUrl = gameSave.SaveFileUrl,
            CreatedAt = gameSave.CreatedAt,
            LastUpdatedAt = gameSave.LastUpdatedAt,
            UserId = gameSave.UserId,
            UserLibraryGameId = gameSave.UserLibraryGameId
        };
    }

    /// <inheritdoc />
    public async Task<GameSave> AddAsync(GameSaveDto gameSaveDto)
    {
        if (gameSaveDto == null)
        {
            throw new ArgumentNullException(nameof(gameSaveDto));
        }

        var gameSave = new GameSave
        {
            Id = gameSaveDto.Id,
            UserId = gameSaveDto.UserId,
            UserLibraryGameId = gameSaveDto.UserLibraryGameId,
            SaveFileUrl = gameSaveDto.SaveFileUrl,
            CreatedAt = gameSaveDto.CreatedAt
        };

        return await _gameSaveRepository.AddAsync(gameSave);
    }

    /// <inheritdoc />
    public async Task<GameSave> UpdateAsync(GameSaveDto gameSaveDto)
    {
        if (gameSaveDto == null)
        {
            throw new ArgumentNullException(nameof(gameSaveDto));
        }
        
        var gameSave = await _gameSaveRepository.GetByIdAsync(gameSaveDto.Id);
        if (gameSave == null)
        {
            throw new InvalidOperationException("Game save not found.");
        }
        
        gameSave.UserId = gameSaveDto.UserId;
        gameSave.UserLibraryGameId = gameSaveDto.UserLibraryGameId;
        gameSave.SaveFileUrl = gameSaveDto.SaveFileUrl;
        gameSave.LastUpdatedAt = gameSaveDto.LastUpdatedAt;

        return await _gameSaveRepository.UpdateAsync(gameSave);
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