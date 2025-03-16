namespace Forja.Application.Services.Games;

/// <summary>
/// A service class responsible for managing operations related to product genres, such as
/// creation, retrieval, updating, and deletion of product-genre associations.
/// </summary>
public class ProductGenresService : IProductGenresService
{
    private readonly IProductGenresRepository _productGenresRepository;

    public ProductGenresService(IProductGenresRepository productGenresRepository)
    {
        _productGenresRepository = productGenresRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenresDto>> GetAllAsync()
    {
        var productGenres = await _productGenresRepository.GetAllAsync();

        return productGenres.Select(GamesEntityToDtoMapper.MapToProductGenresDto);
    }

    /// <inheritdoc />
    public async Task<ProductGenresDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var productGenre = await _productGenresRepository.GetByIdAsync(id);

        return productGenre == null ? null : GamesEntityToDtoMapper.MapToProductGenresDto(productGenre);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenresDto>> GetByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }
        var productGenresByProduct = await _productGenresRepository.GetByProductIdAsync(productId);

        return productGenresByProduct.Select(GamesEntityToDtoMapper.MapToProductGenresDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductGenresDto>> GetByGenreIdAsync(Guid genreId)
    {
        if (genreId == Guid.Empty)
        {
            throw new ArgumentException("Genre ID cannot be empty.", nameof(genreId));
        }
        var productGenresByGenre = await _productGenresRepository.GetByGenreIdAsync(genreId);

        return productGenresByGenre.Select(GamesEntityToDtoMapper.MapToProductGenresDto);
    }

    /// <inheritdoc />
    public async Task<ProductGenresDto?> CreateAsync(ProductGenresCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateProductGenresCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newProductGenre = new ProductGenres
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            GenreId = request.GenreId
        };

        var createdProductGenre = await _productGenresRepository.AddAsync(newProductGenre);

        return createdProductGenre == null ? null : GamesEntityToDtoMapper.MapToProductGenresDto(createdProductGenre);
    }

    /// <inheritdoc />
    public async Task<ProductGenresDto?> UpdateAsync(ProductGenresUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateProductGenresUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingProductGenre = await _productGenresRepository.GetByIdAsync(request.Id);
        if (existingProductGenre == null)
        {
            throw new KeyNotFoundException($"ProductGenre with ID {request.Id} not found.");
        }
        
        existingProductGenre.ProductId = request.ProductId;
        existingProductGenre.GenreId = request.GenreId;
        
        var updatedProductGenre = await _productGenresRepository.UpdateAsync(existingProductGenre);
        
        return updatedProductGenre == null ? null : GamesEntityToDtoMapper.MapToProductGenresDto(updatedProductGenre);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var existingProductGenre = await _productGenresRepository.GetByIdAsync(id);
        if (existingProductGenre == null)
        {
            throw new KeyNotFoundException($"ProductGenre with ID {id} not found.");
        }

        await _productGenresRepository.DeleteAsync(id);
    }
}