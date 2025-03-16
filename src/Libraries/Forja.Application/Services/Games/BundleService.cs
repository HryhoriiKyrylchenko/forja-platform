namespace Forja.Application.Services.Games;

/// <summary>
/// Provides operations related to managing game bundles including
/// retrieving, creating, updating, and deleting bundles.
/// </summary>
public class BundleService : IBundleService
{
    private readonly IBundleRepository _bundleRepository;

    public BundleService(IBundleRepository bundleRepository)
    {
        _bundleRepository = bundleRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleDto>> GetAllAsync()
    {
        var bundles = await _bundleRepository.GetAllAsync();
        return bundles.Select(GamesEntityToDtoMapper.MapToBundleDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleDto>> GetActiveBundlesAsync()
    {
        var activeBundles = await _bundleRepository.GetActiveBundlesAsync();
        return activeBundles.Select(GamesEntityToDtoMapper.MapToBundleDto);
    }

    /// <inheritdoc />
    public async Task<BundleDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var bundle = await _bundleRepository.GetByIdAsync(id);
        return bundle == null ? null : GamesEntityToDtoMapper.MapToBundleDto(bundle);
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
        return createdBundle == null ? null : GamesEntityToDtoMapper.MapToBundleDto(createdBundle);
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
        return updatedBundle == null ? null : GamesEntityToDtoMapper.MapToBundleDto(updatedBundle);
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