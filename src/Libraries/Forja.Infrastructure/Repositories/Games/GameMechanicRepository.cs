namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameMechanicRepository interface using Entity Framework Core.
/// </summary>
public class GameMechanicRepository : IGameMechanicRepository
{
    private readonly DbContext _context;
    private readonly DbSet<GameMechanic> _gameMechanics;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameMechanicRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameMechanicRepository(DbContext context)
    {
        _context = context;
        _gameMechanics = context.Set<GameMechanic>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanic>> GetAllAsync()
    {
        return await _gameMechanics
            .Include(gm => gm.Game) // Include associated Game
            .Include(gm => gm.Mechanic) // Include associated Mechanic
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameMechanic?> GetByIdAsync(Guid id)
    {
        return await _gameMechanics
            .Include(gm => gm.Game) // Include associated Game
            .Include(gm => gm.Mechanic) // Include associated Mechanic
            .FirstOrDefaultAsync(gm => gm.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanic>> GetByGameIdAsync(Guid gameId)
    {
        return await _gameMechanics
            .Where(gm => gm.GameId == gameId)
            .Include(gm => gm.Mechanic) // Include associated Mechanic
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameMechanic>> GetByMechanicIdAsync(Guid mechanicId)
    {
        return await _gameMechanics
            .Where(gm => gm.MechanicId == mechanicId)
            .Include(gm => gm.Game) // Include associated Game
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameMechanic?> AddAsync(GameMechanic gameMechanic)
    {
        await _gameMechanics.AddAsync(gameMechanic);
        await _context.SaveChangesAsync();
        return gameMechanic;
    }

    /// <inheritdoc />
    public async Task<GameMechanic?> UpdateAsync(GameMechanic gameMechanic)
    {
        _gameMechanics.Update(gameMechanic);
        await _context.SaveChangesAsync();
        return gameMechanic;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var gameMechanic = await _gameMechanics.FindAsync(id);
        if (gameMechanic != null)
        {
            _gameMechanics.Remove(gameMechanic);
            await _context.SaveChangesAsync();
        }
    }
}