namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IMatureContentRepository interface using Entity Framework Core.
/// </summary>
public class MatureContentRepository : IMatureContentRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<MatureContent> _matureContents;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatureContentRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public MatureContentRepository(ForjaDbContext context)
    {
        _context = context;
        _matureContents = context.Set<MatureContent>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MatureContent>> GetAllAsync()
    {
        return await _matureContents
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<MatureContent?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid mature content ID.", nameof(id));
        }
        
        return await _matureContents
            .FirstOrDefaultAsync(mc => mc.Id == id);
    }

    /// <inheritdoc />
    public async Task<MatureContent?> AddAsync(MatureContent matureContent)
    {
        if (!GamesModelValidator.ValidateMatureContent(matureContent, out _))
        {
            throw new ArgumentException("Invalid mature content.", nameof(matureContent));
        }
        
        await _matureContents.AddAsync(matureContent);
        await _context.SaveChangesAsync();
        return matureContent;
    }

    /// <inheritdoc />
    public async Task<MatureContent?> UpdateAsync(MatureContent matureContent)
    {
        if (!GamesModelValidator.ValidateMatureContent(matureContent, out _))
        {
            throw new ArgumentException("Invalid mature content.", nameof(matureContent));
        }

        _matureContents.Update(matureContent);
        await _context.SaveChangesAsync();
        return matureContent;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid mature content ID.", nameof(id));
        }
        
        var matureContent = await _matureContents.FindAsync(id);
        if (matureContent == null)
        {
            throw new ArgumentException("Mature content not found.", nameof(id));
        }
        
        _matureContents.Remove(matureContent);
        await _context.SaveChangesAsync();
    }
}