namespace Forja.Application.Services.Games;

public class ProductVersionService : IProductVersionService
{
    private readonly IProductVersionRepository _productVersionRepository;

    public ProductVersionService(IProductVersionRepository productVersionRepository)
    {
        _productVersionRepository = productVersionRepository;
    }
    
    ///<inheritdoc/>
    public async Task<ProductVersionDto?> AddProductVersionAsync(ProductVersionCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameVersionCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        var gameVersion = new ProductVersion
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            Platform = request.Platform,
            Version = request.Version,
            StorageUrl = request.StorageUrl,
            Changelog = request.Changelog,
            ReleaseDate = request.ReleaseDate
        };
        
        var result = await _productVersionRepository.AddAsync(gameVersion);
        return result == null ? null : GamesEntityToDtoMapper.MapToProductVersionDto(result);
    }

    ///<inheritdoc/>
    public async Task<ProductVersionDto?> UpdateProductVersionAsync(ProductVersionUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameVersionUpdateRequest(request, out var errors))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingProductVersion = await _productVersionRepository.GetByIdAsync(request.Id);
        if (existingProductVersion == null)
        {
            throw new KeyNotFoundException($"GameVersion with ID {request.Id} not found.");
        }

        existingProductVersion.Changelog = request.ChangeLog;
        existingProductVersion.ReleaseDate = request.ReleaseDate;
        
        var result = await _productVersionRepository.UpdateAsync(existingProductVersion);
        return result == null ? null : GamesEntityToDtoMapper.MapToProductVersionDto(result);
    }

    ///<inheritdoc/>
    public async Task<ProductVersionDto?> GetProductVersionByProductIdPlatformAndVersionAsync(Guid productId, PlatformType platform, string version)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(productId));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("Invalid version.", nameof(version));
        }
        
        var result = await _productVersionRepository.GetByProductIdPlatformAndVersionAsync(productId, platform, version);
        return result == null ? null : GamesEntityToDtoMapper.MapToProductVersionDto(result);
    }
}