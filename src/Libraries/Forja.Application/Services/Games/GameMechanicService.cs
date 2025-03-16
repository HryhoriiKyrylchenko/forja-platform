namespace Forja.Application.Services.Games;

/// <summary>
/// Service class for managing game mechanics within the application.
/// Provides functionality to create, retrieve, update, and delete game mechanics.
/// </summary>
public class GameMechanicService : IGameMechanicService

{
    private readonly IGameMechanicRepository _gameMechanicRepository;

    public GameMechanicService(IGameMechanicRepository gameMechanicRepository)
    {
        _gameMechanicRepository = gameMechanicRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanicDto>> GetAllAsync()
    {
        var gameMechanics = await _gameMechanicRepository.GetAllAsync();

        return gameMechanics.Select(GamesEntityToDtoMapper.MapToGameMechanicDto);
    }

    /// <inheritdoc />
    public async Task<GameMechanicDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var gameMechanic = await _gameMechanicRepository.GetByIdAsync(id);
 
        return gameMechanic == null ? null : GamesEntityToDtoMapper.MapToGameMechanicDto(gameMechanic);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanicDto>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameId));
        }
        var gameMechanics = await _gameMechanicRepository.GetByGameIdAsync(gameId);

        return gameMechanics.Select(GamesEntityToDtoMapper.MapToGameMechanicDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanicDto>> GetByMechanicIdAsync(Guid mechanicId)
    {
        if (mechanicId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(mechanicId));
        }
        var gameMechanics = await _gameMechanicRepository.GetByMechanicIdAsync(mechanicId);

        return gameMechanics.Select(GamesEntityToDtoMapper.MapToGameMechanicDto);
    }

    /// <inheritdoc />
    public async Task<GameMechanicDto?> CreateAsync(GameMechanicCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameMechanicCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newGameMechanic = new GameMechanic
        {
            Id = Guid.NewGuid(),
            GameId = request.GameId,
            MechanicId = request.MechanicId
        };

        var createdGameMechanic = await _gameMechanicRepository.AddAsync(newGameMechanic);

        return createdGameMechanic == null ? null : GamesEntityToDtoMapper.MapToGameMechanicDto(createdGameMechanic);
    }

    /// <inheritdoc />
    public async Task<GameMechanicDto?> UpdateAsync(GameMechanicUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameMechanicUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingGameMechanic = await _gameMechanicRepository.GetByIdAsync(request.Id);
        if (existingGameMechanic == null)
        {
            throw new KeyNotFoundException($"GameMechanic with ID {request.Id} not found.");
        }

        existingGameMechanic.GameId = request.GameId;
        existingGameMechanic.MechanicId = request.MechanicId;

        var updatedGameMechanic = await _gameMechanicRepository.UpdateAsync(existingGameMechanic);

        return updatedGameMechanic == null ? null : GamesEntityToDtoMapper.MapToGameMechanicDto(updatedGameMechanic);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var existingGameMechanic = await _gameMechanicRepository.GetByIdAsync(id);
        if (existingGameMechanic == null)
        {
            throw new KeyNotFoundException($"GameMechanic with ID {id} not found.");
        }

        await _gameMechanicRepository.DeleteAsync(id);
    }
}