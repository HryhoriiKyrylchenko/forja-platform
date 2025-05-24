namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the ITagRepository interface using Entity Framework Core.
/// </summary>
public class TagRepository : ITagRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Tag> _tags;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to use.</param>
    public TagRepository(ForjaDbContext context)
    {
        _context = context;
        _tags = context.Set<Tag>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _tags
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid tag ID.", nameof(id));
        }
        
        return await _tags
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc />
    public async Task<Tag?> AddAsync(Tag tag)
    {
        if (!GamesModelValidator.ValidateTag(tag, out _))
        {
            throw new ArgumentException("Invalid tag.", nameof(tag));
        }
        
        await _tags.AddAsync(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    /// <inheritdoc />
    public async Task<Tag?> UpdateAsync(Tag tag)
    {
        if (!GamesModelValidator.ValidateTag(tag, out _))
        {
            throw new ArgumentException("Invalid tag.", nameof(tag));
        }

        _tags.Update(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid tag ID.", nameof(id));
        }
        
        var tag = await _tags.FindAsync(id);
        if (tag == null)
        {
            throw new ArgumentException("Tag not found.", nameof(id));
        }
        
        _tags.Remove(tag);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Tag>> GetAllWithDetailsAsync()
    {
        return await _tags
            .Include(t => t.GameTags)
            .ToListAsync();
    }
}