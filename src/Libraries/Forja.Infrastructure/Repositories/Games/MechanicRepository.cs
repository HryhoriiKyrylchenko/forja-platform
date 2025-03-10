namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IMechanicRepository interface using Entity Framework Core.
/// </summary>
public class MechanicRepository : IMechanicRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Mechanic> _mechanics;

    /// <summary>
    /// Initializes a new instance of the <see cref="MechanicRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public MechanicRepository(DbContext context)
    {
        _context = context;
        _mechanics = context.Set<Mechanic>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Mechanic>> GetAllAsync()
    {
        return await _mechanics
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Mechanic?> GetByIdAsync(Guid id)
    {
        return await _mechanics
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <inheritdoc />
    public async Task<Mechanic?> AddAsync(Mechanic mechanic)
    {
        await _mechanics.AddAsync(mechanic);
        await _context.SaveChangesAsync();
        return mechanic;
    }

    /// <inheritdoc />
    public async Task<Mechanic?> UpdateAsync(Mechanic mechanic)
    {
        _mechanics.Update(mechanic);
        await _context.SaveChangesAsync();
        return mechanic;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var mechanic = await _mechanics.FindAsync(id);
        if (mechanic != null)
        {
            _mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Mechanic>> GetAllWithGameMechanicsAsync()
    {
        return await _mechanics
            .Include(m => m.GameMechanics) // Include the GameMechanics relationship
            .ThenInclude(gm => gm.Game)   // Optionally include the associated Game entities
            .ToListAsync();
    }
}
