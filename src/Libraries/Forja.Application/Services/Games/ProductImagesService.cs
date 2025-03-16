namespace Forja.Application.Services.Games;

/// <summary>
/// Service implementation for managing ProductImages entities.
/// </summary>
public class ProductImagesService : IProductImagesService
{
    private readonly IProductImagesRepository _productImagesRepository;

    public ProductImagesService(IProductImagesRepository productImagesRepository)
    {
        _productImagesRepository = productImagesRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImagesDto>> GetAllAsync()
    {
        var productImages = await _productImagesRepository.GetAllAsync();

        return productImages.Select(GamesEntityToDtoMapper.MapToProductImagesDto);
    }

    /// <inheritdoc />
    public async Task<ProductImagesDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var productImage = await _productImagesRepository.GetByIdAsync(id);

        return productImage == null ? null : GamesEntityToDtoMapper.MapToProductImagesDto(productImage);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImagesDto>> GetAllWithDetailsAsync()
    {
        var productImagesWithDetails = await _productImagesRepository.GetAllWithDetailsAsync();

        return productImagesWithDetails.Select(GamesEntityToDtoMapper.MapToProductImagesDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImagesDto>> GetByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }
        var productImagesByProduct = await _productImagesRepository.GetByProductIdAsync(productId);

        return productImagesByProduct.Select(GamesEntityToDtoMapper.MapToProductImagesDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImagesDto>> GetByItemImageIdAsync(Guid itemImageId)
    {
        if (itemImageId == Guid.Empty)
        {
            throw new ArgumentException("ItemImage ID cannot be empty.", nameof(itemImageId));
        }
        var productImagesByItemImage = await _productImagesRepository.GetByItemImageIdAsync(itemImageId);

        return productImagesByItemImage.Select(GamesEntityToDtoMapper.MapToProductImagesDto);
    }

    /// <inheritdoc />
    public async Task<ProductImagesDto?> CreateAsync(ProductImagesCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateProductImagesCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newProductImage = new ProductImages
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            ItemImageId = request.ItemImageId
        };

        var createdProductImage = await _productImagesRepository.AddAsync(newProductImage);

        return createdProductImage == null ? null : GamesEntityToDtoMapper.MapToProductImagesDto(createdProductImage);
    }

    /// <inheritdoc />
    public async Task<ProductImagesDto?> UpdateAsync(ProductImagesUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateProductImagesUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingProductImage = await _productImagesRepository.GetByIdAsync(request.Id);
        if (existingProductImage == null)
        {
            throw new KeyNotFoundException($"ProductImage with ID {request.Id} not found.");
        }

        existingProductImage.ProductId = request.ProductId;
        existingProductImage.ItemImageId = request.ItemImageId;

        var updatedProductImage = await _productImagesRepository.UpdateAsync(existingProductImage);

        return updatedProductImage == null ? null : GamesEntityToDtoMapper.MapToProductImagesDto(updatedProductImage);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var existingProductImage = await _productImagesRepository.GetByIdAsync(id);
        if (existingProductImage == null)
        {
            throw new KeyNotFoundException($"ProductImage with ID {id} not found.");
        }

        await _productImagesRepository.DeleteAsync(id);
    }
}