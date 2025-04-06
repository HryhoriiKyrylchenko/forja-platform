namespace Forja.Application.Services.Games;

/// <summary>
/// Provides operations related to managing game bundles including
/// retrieving, creating, updating, and deleting bundles.
/// </summary>
public class BundleService : IBundleService
{
    private readonly IBundleRepository _bundleRepository;
    private readonly IFileManagerService _fileManagerService;

    public BundleService(IBundleRepository bundleRepository,
        IFileManagerService fileManagerService)
    {
        _bundleRepository = bundleRepository;
        _fileManagerService = fileManagerService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleDto>> GetAllAsync()
    {
        var allBundles = await _bundleRepository.GetAllActiveAsync();

        var bundles = new List<BundleDto>();

        foreach (var bundle in allBundles)
        {
            var bundleProductDtos = new List<BundleProductDto>();
            foreach (var bundleProduct in bundle.BundleProducts)
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bundleProduct.ProductId);
                var mappedProductDto = GamesEntityToDtoMapper.MapToBundleProductDto(bundleProduct, logoUrl);
                bundleProductDtos.Add(mappedProductDto);
            }

            var bundleDto = GamesEntityToDtoMapper.MapToBundleDto(bundle, bundleProductDtos);
            bundles.Add(bundleDto);
        }

        return bundles;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleDto>> GetActiveBundlesAsync()
    {
        var activeBundles = await _bundleRepository.GetActiveBundlesAsync();

        var bundles = new List<BundleDto>();

        foreach (var bundle in activeBundles)
        {
            var bundleProductDtos = new List<BundleProductDto>();
            foreach (var bundleProduct in bundle.BundleProducts)
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bundleProduct.ProductId);

                var bundleProductDto = GamesEntityToDtoMapper.MapToBundleProductDto(bundleProduct, logoUrl);

                bundleProductDtos.Add(bundleProductDto);
            }

            var bundleDto = GamesEntityToDtoMapper.MapToBundleDto(bundle, bundleProductDtos);
            bundles.Add(bundleDto);
        }

        return bundles;
    }

    /// <inheritdoc />
    public async Task<BundleDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        var bundle = await _bundleRepository.GetByIdAsync(id);
        if (bundle == null)
        {
            return null;
        }

        var bundleProductDtos = new List<BundleProductDto>();
        foreach (var bundleProduct in bundle.BundleProducts)
        {
            var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bundleProduct.Product.Id);

            var bundleProductDto = GamesEntityToDtoMapper.MapToBundleProductDto(bundleProduct, logoUrl);

            bundleProductDtos.Add(bundleProductDto);
        }

        var bundleDto = GamesEntityToDtoMapper.MapToBundleDto(bundle, bundleProductDtos);

        return bundleDto;
    }

    /// <inheritdoc />
    public async Task<BundleDto?> CreateAsync(BundleCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateBundleCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var newBundle = new Bundle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            TotalPrice = request.TotalPrice,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = request.ExpirationDate,
            IsActive = true
        };

        var createdBundle = await _bundleRepository.AddAsync(newBundle);
        if (createdBundle == null)
        {
            return null;
        }

        var bundleProductDtos = await Task.WhenAll(
            createdBundle.BundleProducts.Select(async bp =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.Product.Id);
                return GamesEntityToDtoMapper.MapToBundleProductDto(bp, logoUrl);
            })
        );

        var bundleDto = GamesEntityToDtoMapper.MapToBundleDto(createdBundle, bundleProductDtos.ToList());
        return bundleDto;
    }

    /// <inheritdoc />
    public async Task<BundleDto?> UpdateAsync(BundleUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateBundleUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingBundle = await _bundleRepository.GetByIdAsync(request.Id);
        if (existingBundle == null) return null;

        existingBundle.Title = request.Title;
        existingBundle.Description = request.Description;
        existingBundle.TotalPrice = request.TotalPrice;
        existingBundle.IsActive = request.IsActive;

        var updatedBundle = await _bundleRepository.UpdateAsync(existingBundle);
        if (updatedBundle == null)
        {
            return null;
        }

        var bundleProductDtos = await Task.WhenAll(
            updatedBundle.BundleProducts.Select(async bp =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.Product.Id);
                return GamesEntityToDtoMapper.MapToBundleProductDto(bp, logoUrl);
            })
        );

        var bundleDto = GamesEntityToDtoMapper.MapToBundleDto(updatedBundle, bundleProductDtos.ToList());
        return bundleDto;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        await _bundleRepository.DeleteAsync(id);
    }
}