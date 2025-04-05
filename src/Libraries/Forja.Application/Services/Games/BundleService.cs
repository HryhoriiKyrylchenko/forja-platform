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
        var bundles = await Task.WhenAll(allBundles.Select(async b =>
        {
            var bundleProductDtos = await Task.WhenAll(b.BundleProducts.Select(async bp =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.ProductId);
                return GamesEntityToDtoMapper.MapToBundleProductDto(bp, logoUrl);
            }));

            return GamesEntityToDtoMapper.MapToBundleDto(b, bundleProductDtos.ToList());
        }));
        
        return bundles;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleDto>> GetActiveBundlesAsync()
    {
        var activeBundles = await _bundleRepository.GetActiveBundlesAsync();
        var bundles = await Task.WhenAll(activeBundles.Select(async b =>
        {
            var bundleProductDtos = await Task.WhenAll(b.BundleProducts.Select(async bp =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.ProductId);
                return GamesEntityToDtoMapper.MapToBundleProductDto(bp, logoUrl);
            }));

            return GamesEntityToDtoMapper.MapToBundleDto(b, bundleProductDtos.ToList());
        }));
        
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

        var bundleProductDtos = await Task.WhenAll(
            bundle.BundleProducts.Select(async bp =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(bp.Product.Id);
                return GamesEntityToDtoMapper.MapToBundleProductDto(bp, logoUrl);
            })
        );

        var bundleDto = GamesEntityToDtoMapper.MapToBundleDto(bundle, bundleProductDtos.ToList());
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