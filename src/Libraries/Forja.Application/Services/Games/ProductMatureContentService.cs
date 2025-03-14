namespace Forja.Application.Services.Games;

/// <summary>
/// Provides operations for handling mature content associated with products.
/// </summary>
public class ProductMatureContentService : IProductMatureContentService
{
    private readonly IProductMatureContentRepository _productMatureContentRepository;

    public ProductMatureContentService(IProductMatureContentRepository productMatureContentRepository)
    {
        _productMatureContentRepository = productMatureContentRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContentDto>> GetAllAsync()
    {
        var productMatureContents = await _productMatureContentRepository.GetAllAsync();

        return productMatureContents.Select(GamesEntityToDtoMapper.MapToProductMatureContentDto);
    }

    /// <inheritdoc />
    public async Task<ProductMatureContentDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var productMatureContent = await _productMatureContentRepository.GetByIdAsync(id);

        return productMatureContent == null ? null : GamesEntityToDtoMapper.MapToProductMatureContentDto(productMatureContent);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContentDto>> GetByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }
        var productMatureContentsByProduct = await _productMatureContentRepository.GetByProductIdAsync(productId);

        return productMatureContentsByProduct.Select(GamesEntityToDtoMapper.MapToProductMatureContentDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContentDto>> GetByMatureContentIdAsync(Guid matureContentId)
    {
        if (matureContentId == Guid.Empty)
        {
            throw new ArgumentException("MatureContent ID cannot be empty.", nameof(matureContentId));
        }
        var productMatureContentsByMatureContent = await _productMatureContentRepository.GetByMatureContentIdAsync(matureContentId);

        return productMatureContentsByMatureContent.Select(GamesEntityToDtoMapper.MapToProductMatureContentDto);
    }

    /// <inheritdoc />
    public async Task<ProductMatureContentDto?> CreateAsync(ProductMatureContentCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateProductMatureContentCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newProductMatureContent = new ProductMatureContent
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            MatureContentId = request.MatureContentId
        };

        var createdProductMatureContent = await _productMatureContentRepository.AddAsync(newProductMatureContent);

        return createdProductMatureContent == null ? null : GamesEntityToDtoMapper.MapToProductMatureContentDto(createdProductMatureContent);
    }

    /// <inheritdoc />
    public async Task<ProductMatureContentDto?> UpdateAsync(ProductMatureContentUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateProductMatureContentUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingProductMatureContent = await _productMatureContentRepository.GetByIdAsync(request.Id);
        if (existingProductMatureContent == null)
        {
            throw new KeyNotFoundException($"ProductMatureContent with ID {request.Id} not found.");
        }

        existingProductMatureContent.ProductId = request.ProductId;
        existingProductMatureContent.MatureContentId = request.MatureContentId;

        var updatedProductMatureContent = await _productMatureContentRepository.UpdateAsync(existingProductMatureContent);

        return updatedProductMatureContent == null ? null : GamesEntityToDtoMapper.MapToProductMatureContentDto(updatedProductMatureContent);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var existingProductMatureContent = await _productMatureContentRepository.GetByIdAsync(id);
        if (existingProductMatureContent == null)
        {
            throw new KeyNotFoundException($"ProductMatureContent with ID {id} not found.");
        }

        await _productMatureContentRepository.DeleteAsync(id);
    }
}