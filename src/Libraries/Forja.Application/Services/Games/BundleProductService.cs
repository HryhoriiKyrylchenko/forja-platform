namespace Forja.Application.Services.Games;

/// <summary>
/// Represents a service for managing bundle products in the gaming domain.
/// </summary>
/// <remarks>
/// This service provides operations for retrieving, creating, updating, and deleting bundle products. It interacts with
/// the bundle product repository for data operations and performs necessary validations before executing any operation.
/// </remarks>
public class BundleProductService : IBundleProductService
{
    private readonly IBundleProductRepository _bundleProductRepository;
    private readonly IFileManagerService _fileManagerService;

    public BundleProductService(IBundleProductRepository bundleProductRepository,
        IFileManagerService fileManagerService)
    {
        _bundleProductRepository = bundleProductRepository;
        _fileManagerService = fileManagerService;
    }

    /// <inheritdoc />
    public async Task<List<BundleProductDto>> GetAllAsync()
    {
        var bundleProducts = await _bundleProductRepository.GetAllAsync();

        var result = await Task.WhenAll(bundleProducts.Select(async bp => GamesEntityToDtoMapper.MapToBundleProductDto(
            bp,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.ProductId, 1900)
        )));
        
        return result.ToList();
    }

    /// <inheritdoc />
    public async Task<BundleProductDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var bundleProduct = await _bundleProductRepository.GetByIdAsync(id);
        
        return bundleProduct == null ? null : GamesEntityToDtoMapper.MapToBundleProductDto(
            bundleProduct,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(bundleProduct.ProductId, 1900));
    }

    /// <inheritdoc />
    public async Task<List<BundleProductDto>> GetByBundleIdAsync(Guid bundleId)
    {
        if (bundleId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(bundleId));
        }
        var bundleProducts = await _bundleProductRepository.GetByBundleIdAsync(bundleId);

        var result = await Task.WhenAll(bundleProducts.Select(async bp => GamesEntityToDtoMapper.MapToBundleProductDto(
            bp,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.ProductId, 1900)
        )));
        
        return result.ToList();
    }

    /// <inheritdoc />
    public async Task<BundleProductDto?> CreateAsync(BundleProductCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateBundleProductCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newBundleProduct = new BundleProduct
        {
            Id = Guid.NewGuid(),
            BundleId = request.BundleId,
            ProductId = request.ProductId
        };

        var createdBundleProduct = await _bundleProductRepository.AddAsync(newBundleProduct);

        return createdBundleProduct == null ? null : GamesEntityToDtoMapper.MapToBundleProductDto(
            createdBundleProduct,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(createdBundleProduct.ProductId, 1900));
    }

    /// <inheritdoc />
    public async Task<BundleProductDto?> UpdateAsync(BundleProductUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateBundleProductUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingBundleProduct = await _bundleProductRepository.GetByIdAsync(request.Id);
        if (existingBundleProduct == null)
        {
            throw new KeyNotFoundException($"BundleProduct with ID {request.Id} not found.");
        }

        existingBundleProduct.BundleId = request.BundleId;
        existingBundleProduct.ProductId = request.ProductId;

        var updatedBundleProduct = await _bundleProductRepository.UpdateAsync(existingBundleProduct);

        return updatedBundleProduct == null ? null : GamesEntityToDtoMapper.MapToBundleProductDto(
            updatedBundleProduct,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(updatedBundleProduct.ProductId, 1900));
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var existingBundleProduct = await _bundleProductRepository.GetByIdAsync(id);
        if (existingBundleProduct == null)
        {
            throw new KeyNotFoundException($"BundleProduct with ID {id} not found.");
        }

        await _bundleProductRepository.DeleteAsync(id);
    }
}