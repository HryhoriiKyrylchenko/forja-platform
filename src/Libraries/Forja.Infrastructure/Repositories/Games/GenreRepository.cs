namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGenreRepository interface using Entity Framework Core.
/// </summary>
public class GenreRepository : IGenreRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Genre> _genres;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenreRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GenreRepository(ForjaDbContext context)
    {
        _context = context;
        _genres = context.Set<Genre>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _genres
            .Where(g => !g.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Genre>> GetAllDeletedAsync()
    {
        return await _genres
            .Where(g => g.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Genre?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid genre ID.", nameof(id));
        }
        
        return await _genres
            .Where(g => !g.IsDeleted)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc />
    public async Task<Genre?> AddAsync(Genre genre)
    {
        if (!GamesModelValidator.ValidateGenre(genre, out _))
        {
            throw new ArgumentException("Invalid genre.", nameof(genre));
        }
        
        await _genres.AddAsync(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    /// <inheritdoc />
    public async Task<Genre?> UpdateAsync(Genre genre)
    {
        if (!GamesModelValidator.ValidateGenre(genre, out _))
        {
            throw new ArgumentException("Invalid genre.", nameof(genre));
        }

        _genres.Update(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid genre ID.", nameof(id));
        }
        
        var genre = await _genres.FindAsync(id);
        if (genre == null)
        {
            throw new ArgumentException("Genre not found.", nameof(id));
        }
        
        genre.IsDeleted = true;
        _genres.Update(genre);
        await _context.SaveChangesAsync();
    }
}