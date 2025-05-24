namespace Forja.Application.Services.Games;

/// <summary>
/// Service class responsible for handling operations related to game tags.
/// </summary>
public class GameTagService : IGameTagService
{
    private readonly IGameTagRepository _gameTagRepository;

    public GameTagService(IGameTagRepository gameTagRepository)
    {
        _gameTagRepository = gameTagRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameTagDto>> GetAllAsync()
    {
        var gameTags = await _gameTagRepository.GetAllAsync();

        return gameTags.Select(GamesEntityToDtoMapper.MapToGameTagDto);
    }

    /// <inheritdoc />
    public async Task<GameTagDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var gameTag = await _gameTagRepository.GetByIdAsync(id);

        return gameTag == null ? null : GamesEntityToDtoMapper.MapToGameTagDto(gameTag);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameTagDto>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameId));
        }
        var gameTags = await _gameTagRepository.GetByGameIdAsync(gameId);

        return gameTags.Select(GamesEntityToDtoMapper.MapToGameTagDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameTagDto>> GetByTagIdAsync(Guid tagId)
    {
        if (tagId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(tagId));
        }
        var gameTags = await _gameTagRepository.GetByTagIdAsync(tagId);

        return gameTags.Select(GamesEntityToDtoMapper.MapToGameTagDto);
    }

    /// <inheritdoc />
    public async Task<GameTagDto?> CreateAsync(GameTagCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameTagCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newGameTag = new GameTag
        {
            Id = Guid.NewGuid(),
            GameId = request.GameId,
            TagId = request.TagId
        };

        var createdGameTag = await _gameTagRepository.AddAsync(newGameTag);

        return createdGameTag == null ? null : GamesEntityToDtoMapper.MapToGameTagDto(createdGameTag);
    }

    /// <inheritdoc />
    public async Task<GameTagDto?> UpdateAsync(GameTagUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameTagUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingGameTag = await _gameTagRepository.GetByIdAsync(request.Id);
        if (existingGameTag == null)
        {
            throw new KeyNotFoundException($"GameTag with ID {request.Id} not found.");
        }

        existingGameTag.GameId = request.GameId;
        existingGameTag.TagId = request.TagId;

        var updatedGameTag = await _gameTagRepository.UpdateAsync(existingGameTag);

        return updatedGameTag == null ? null : GamesEntityToDtoMapper.MapToGameTagDto(updatedGameTag);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var existingGameTag = await _gameTagRepository.GetByIdAsync(id);
        if (existingGameTag == null)
        {
            throw new KeyNotFoundException($"GameTag with ID {id} not found.");
        }

        await _gameTagRepository.DeleteAsync(id);
    }
}