namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameMechanicRepository interface using Entity Framework Core.
/// </summary>
public class GameMechanicRepository : IGameMechanicRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<GameMechanic> _gameMechanics;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameMechanicRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameMechanicRepository(ForjaDbContext context)
    {
        _context = context;
        _gameMechanics = context.Set<GameMechanic>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanic>> GetAllAsync()
    {
        return await _gameMechanics
            .Include(gm => gm.Game) 
            .Include(gm => gm.Mechanic) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameMechanic?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        return await _gameMechanics
            .Include(gm => gm.Game) 
            .Include(gm => gm.Mechanic) 
            .FirstOrDefaultAsync(gm => gm.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanic>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be empty.", nameof(gameId));
        }
        
        return await _gameMechanics
            .Where(gm => gm.GameId == gameId)
            .Include(gm => gm.Mechanic) // Include associated Mechanic
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanic>> GetByMechanicIdAsync(Guid mechanicId)
    {
        if (mechanicId == Guid.Empty)
        {
            throw new ArgumentException("Mechanic ID cannot be empty.", nameof(mechanicId));
        }
        
        return await _gameMechanics
            .Where(gm => gm.MechanicId == mechanicId)
            .Include(gm => gm.Game) // Include associated Game
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameMechanic?> AddAsync(GameMechanic gameMechanic)
    {
        if (!GamesModelValidator.ValidateGameMechanic(gameMechanic, out _))
        {
            throw new ArgumentException("Invalid game mechanic.", nameof(gameMechanic));
        }
        
        await _gameMechanics.AddAsync(gameMechanic);
        await _context.SaveChangesAsync();
        return gameMechanic;
    }

    /// <inheritdoc />
    public async Task<GameMechanic?> UpdateAsync(GameMechanic gameMechanic)
    {
        if (!GamesModelValidator.ValidateGameMechanic(gameMechanic, out _))
        {
            throw new ArgumentException("Invalid game mechanic.", nameof(gameMechanic));
        }

        _gameMechanics.Update(gameMechanic);
        await _context.SaveChangesAsync();
        return gameMechanic;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var gameMechanic = await _gameMechanics.FindAsync(id);
        if (gameMechanic == null)
        {
            throw new ArgumentException("Game mechanic not found.", nameof(id));
        }
        
        _gameMechanics.Remove(gameMechanic);
        await _context.SaveChangesAsync();
    }
}