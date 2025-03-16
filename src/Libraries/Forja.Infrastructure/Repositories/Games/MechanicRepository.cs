namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IMechanicRepository interface using Entity Framework Core.
/// </summary>
public class MechanicRepository : IMechanicRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Mechanic> _mechanics;

    /// <summary>
    /// Initializes a new instance of the <see cref="MechanicRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public MechanicRepository(ForjaDbContext context)
    {
        _context = context;
        _mechanics = context.Set<Mechanic>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Mechanic>> GetAllAsync()
    {
        return await _mechanics
            .Where(m => !m.IsDeleted)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Mechanic>> GetAllDeletedAsync()
    {
        return await _mechanics
            .Where(m => m.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Mechanic?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid mechanic ID.", nameof(id));
        }
        
        return await _mechanics
            .Where(m => !m.IsDeleted)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <inheritdoc />
    public async Task<Mechanic?> AddAsync(Mechanic mechanic)
    {
        if (!GamesModelValidator.ValidateMechanic(mechanic, out _))
        {
            throw new ArgumentException("Invalid mechanic.", nameof(mechanic));
        }
        
        await _mechanics.AddAsync(mechanic);
        await _context.SaveChangesAsync();
        return mechanic;
    }

    /// <inheritdoc />
    public async Task<Mechanic?> UpdateAsync(Mechanic mechanic)
    {
        if (!GamesModelValidator.ValidateMechanic(mechanic, out _))
        {
            throw new ArgumentException("Invalid mechanic.", nameof(mechanic));
        }

        _mechanics.Update(mechanic);
        await _context.SaveChangesAsync();
        return mechanic;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid mechanic ID.", nameof(id));
        }
        
        var mechanic = await _mechanics.FindAsync(id);
        if (mechanic == null)
        {
            throw new ArgumentException("Mechanic not found.", nameof(id));
        }
        
        mechanic.IsDeleted = true;
        _mechanics.Update(mechanic);
        await _context.SaveChangesAsync();
    }
}
