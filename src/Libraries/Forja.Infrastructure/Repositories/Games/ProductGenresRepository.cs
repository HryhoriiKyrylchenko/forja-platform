namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IProductGenresRepository interface using Entity Framework Core.
/// </summary>
public class ProductGenresRepository : IProductGenresRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<ProductGenres> _productGenres;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductGenresRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductGenresRepository(ForjaDbContext context)
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
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product genre ID.", nameof(id));
        }
        
        return await _productGenres
            .FirstOrDefaultAsync(pg => pg.Id == id);
    }

    /// <inheritdoc />
    public async Task<ProductGenres?> AddAsync(ProductGenres productGenre)
    {
        if (!GamesModelValidator.ValidateProductGenres(productGenre, out _))
        {
            throw new ArgumentException("Invalid product genre.", nameof(productGenre));
        }
        
        await _productGenres.AddAsync(productGenre);
        await _context.SaveChangesAsync();
        return productGenre;
    }

    /// <inheritdoc />
    public async Task<ProductGenres?> UpdateAsync(ProductGenres productGenre)
    {
        if (!GamesModelValidator.ValidateProductGenres(productGenre, out _))
        {
            throw new ArgumentException("Invalid product genre.", nameof(productGenre));
        }

        _productGenres.Update(productGenre);
        await _context.SaveChangesAsync();
        return productGenre;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product genre ID.", nameof(id));
        }
        
        var productGenre = await _productGenres.FindAsync(id);
        if (productGenre == null)
        {
            throw new ArgumentException("Product genre not found.", nameof(id));
        }
        
        _productGenres.Remove(productGenre);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenres>> GetAllWithDetailsAsync()
    {
        return await _productGenres
            .Include(pg => pg.Product) 
            .Include(pg => pg.Genre)   
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenres>> GetByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID.", nameof(productId));
        }
        
        return await _productGenres
            .Where(pg => pg.ProductId == productId)
            .Include(pg => pg.Genre) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenres>> GetByGenreIdAsync(Guid genreId)
    {
        if (genreId == Guid.Empty)
        {
            throw new ArgumentException("Invalid genre ID.", nameof(genreId));
        }
        
        return await _productGenres
            .Where(pg => pg.GenreId == genreId)
            .Include(pg => pg.Product) 
            .ToListAsync();
    }
}