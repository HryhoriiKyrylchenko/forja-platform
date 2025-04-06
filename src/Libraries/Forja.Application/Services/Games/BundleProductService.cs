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

        var result = new List<BundleProductDto>();

        foreach (var bundleProduct in bundleProducts)
        {
            var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bundleProduct.ProductId, 1900);

            var mappedBundleProductDto = GamesEntityToDtoMapper.MapToBundleProductDto(bundleProduct, logoUrl);

            result.Add(mappedBundleProductDto);
        }

        return result;
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

        var result = new List<BundleProductDto>();

        foreach (var bundleProduct in bundleProducts)
        {
            var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bundleProduct.ProductId, 1900);

            var mappedProductDto = GamesEntityToDtoMapper.MapToBundleProductDto(bundleProduct, logoUrl);

            result.Add(mappedProductDto);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<List<BundleProductDto>> CreateBundleProductsAsync(BundleProductsCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateBundleProductsCreateRequest(request, out var errorMessage))
        {
            throw new ArgumentException($"Invalid request. Error: {errorMessage}", nameof(request));
        }

        if (await _bundleProductRepository.HasBundleProducts(request.BundleId))
        {
            throw new ArgumentException("Bundle already has products.", nameof(request));
        }
        
        var bundleProducts = new List<BundleProduct>();
        foreach (var productId in request.ProductIds)
        {
            bundleProducts.Add(new BundleProduct
            {
                Id = Guid.NewGuid(),
                BundleId = request.BundleId,
                ProductId = productId,
                DistributedPrice = 0m
            });
        }

        var distributedProducts = await _bundleProductRepository.DistributeBundlePrice(bundleProducts, request.BundleTotalPrice);
        if (distributedProducts == null || distributedProducts.Count != bundleProducts.Count)
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var savedProducts = new List<BundleProduct>();
        foreach (var bundleProduct in distributedProducts)
        {
            var savedProduct = await _bundleProductRepository.AddAsync(bundleProduct);
            if (savedProduct == null)
            {
                throw new ArgumentException("Saving bundle product failed.");
            }
            savedProducts.Add(savedProduct);
        }

        var createdBundleProducts = new List<BundleProductDto>();
        foreach (var savedProduct in savedProducts)
        {
            var presignedLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(savedProduct.ProductId, 1900);
            var bundleProductDto = GamesEntityToDtoMapper.MapToBundleProductDto(savedProduct, presignedLogoUrl);
            createdBundleProducts.Add(bundleProductDto);
        }

        return createdBundleProducts;
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