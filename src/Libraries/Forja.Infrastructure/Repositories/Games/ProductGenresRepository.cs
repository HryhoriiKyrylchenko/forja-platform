namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IProductGenresRepository interface using Entity Framework Core.
/// </summary>
public class ProductGenresRepository : IProductGenresRepository
{
    private readonly DbContext _context;
    private readonly DbSet<ProductGenres> _productGenres;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductGenresRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductGenresRepository(DbContext context)
    {
        _context = context;
        _productGenres = context.Set<ProductGenres>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenres>> GetAllAsync()
    {
        return await _productGenres
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductGenres?> GetByIdAsync(Guid id)
    {
        return await _productGenres
            .FirstOrDefaultAsync(pg => pg.Id == id);
    }

    /// <inheritdoc />
    public async Task<ProductGenres?> AddAsync(ProductGenres productGenre)
    {
        await _productGenres.AddAsync(productGenre);
        await _context.SaveChangesAsync();
        return productGenre;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var productGenre = await _productGenres.FindAsync(id);
        if (productGenre != null)
        {
            _productGenres.Remove(productGenre);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenres>> GetAllWithDetailsAsync()
    {
        return await _productGenres
            .Include(pg => pg.Product) // Include the products
            .Include(pg => pg.Genre)   // Include the genres
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenres>> GetByProductIdAsync(Guid productId)
    {
        return await _productGenres
            .Where(pg => pg.ProductId == productId)
            .Include(pg => pg.Genre) // Optionally include genre details
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenres>> GetByGenreIdAsync(Guid genreId)
    {
        return await _productGenres
            .Where(pg => pg.GenreId == genreId)
            .Include(pg => pg.Product) // Optionally include product details
            .ToListAsync();
    }
}