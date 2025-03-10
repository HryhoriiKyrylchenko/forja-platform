namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGenreRepository interface using Entity Framework Core.
/// </summary>
public class GenreRepository : IGenreRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Genre> _genres;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenreRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GenreRepository(DbContext context)
    {
        _context = context;
        _genres = context.Set<Genre>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _genres
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Genre?> GetByIdAsync(Guid id)
    {
        return await _genres
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc />
    public async Task<Genre?> AddAsync(Genre genre)
    {
        await _genres.AddAsync(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    /// <inheritdoc />
    public async Task<Genre?> UpdateAsync(Genre genre)
    {
        _genres.Update(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var genre = await _genres.FindAsync(id);
        if (genre != null)
        {
            _genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Genre>> GetAllWithGamesAsync()
    {
        return await _genres
            .Include(g => g.ProductGenres) // Include associated ProductGenres
            .ThenInclude(pg => pg.Product) 
            .ToListAsync();
    }
}