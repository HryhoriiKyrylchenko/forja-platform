namespace Forja.Application.Services.Games;

/// <summary>
/// Service class responsible for handling game addon operations.
/// </summary>
public class GameAddonService : IGameAddonService
{
    private readonly IGameAddonRepository _gameAddonRepository;
    private readonly IFileManagerService _fileManagerService;
    private readonly IReviewRepository _reviewRepository;

    public GameAddonService(IGameAddonRepository gameAddonRepository,
        IFileManagerService fileManagerService,
        IReviewRepository reviewRepository)
    {
        _gameAddonRepository = gameAddonRepository;
        _fileManagerService = fileManagerService;
        _reviewRepository = reviewRepository;
    }

    /// <inheritdoc />
    public async Task<List<GameAddonDto>> GetAllAsync()
    {
        var addons = await _gameAddonRepository.GetAllAsync();
        
        var allAddonsDto = await Task.WhenAll(addons.Select(async addon =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(addon.Id, 1900);

                var matureContentDtos = await Task.WhenAll(
                    addon.ProductMatureContents.Select(async pmc =>
                    {
                        var matureLogoUrl = pmc.MatureContent.LogoUrl != null
                            ? await _fileManagerService.GetPresignedMatureContentImageUrlAsync(pmc.MatureContent.Id, 1900)
                            : string.Empty;

                        return GamesEntityToDtoMapper.MapToMatureContentDto(pmc.MatureContent, matureLogoUrl);
                    })
                );

                var rating = await _reviewRepository.GetProductApprovedReviewsCountAsync(addon.Id);

                return GamesEntityToDtoMapper.MapToGameAddonDto(
                    addon,
                    logoUrl,
                    matureContentDtos.ToList(),
                    rating
                );
            }
            ));
        
        return allAddonsDto.ToList();
    }

    /// <inheritdoc />
    public async Task<List<GameAddonShortDto>> GetAllForCatalogAsync()
    {
        var addons = await _gameAddonRepository.GetAllForCatalogAsync();
        var gameCatalogDtos = await Task.WhenAll(addons.Select(async a => 
            GamesEntityToDtoMapper.MapToGameAddonShortDto(
                a,
                await _fileManagerService.GetPresignedProductLogoUrlAsync(a.Id, 1900)
            )));

        return gameCatalogDtos.ToList();
    }

    /// <inheritdoc />
    public async Task<GameAddonDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var addon = await _gameAddonRepository.GetByIdAsync(id);
        if (addon == null) return null;

        var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(addon.Id, 1900);

        var matureContentDtos = await Task.WhenAll(
            addon.ProductMatureContents.Select(async pmc =>
            {
                var contentLogoUrl = pmc.MatureContent.LogoUrl != null
                    ? await _fileManagerService.GetPresignedMatureContentImageUrlAsync(pmc.MatureContent.Id, 1900)
                    : string.Empty;

                return GamesEntityToDtoMapper.MapToMatureContentDto(pmc.MatureContent, contentLogoUrl);
            })
        );

        var rating = await _reviewRepository.GetProductApprovedReviewsCountAsync(addon.Id);

        return GamesEntityToDtoMapper.MapToGameAddonDto(
            addon,
            logoUrl,
            matureContentDtos.ToList(),
            rating
        );
    }

    /// <inheritdoc />
    public async Task<GameAddonDto?> CreateAsync(GameAddonCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameAddonCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var newAddon = new GameAddon
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Developer = request.Developer,
            MinimalAge = (MinimalAge)request.MinimalAge,
            Platforms = request.Platforms,
            Price = request.Price,
            LogoUrl = request.LogoUrl,
            ReleaseDate = request.ReleaseDate,
            IsActive = request.IsActive,
            InterfaceLanguages = request.InterfaceLanguages,
            AudioLanguages = request.AudioLanguages,
            SubtitlesLanguages = request.SubtitlesLanguages,
            GameId = request.GameId,
            StorageUrl = request.StorageUrl,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var createdAddon = await _gameAddonRepository.AddAsync(newAddon);
        if (createdAddon == null)
            return null;

        var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(createdAddon.Id, 1900);

        var matureContentDtos = await Task.WhenAll(
            createdAddon.ProductMatureContents.Select(async pmc =>
            {
                var contentLogoUrl = pmc.MatureContent.LogoUrl != null
                    ? await _fileManagerService.GetPresignedMatureContentImageUrlAsync(pmc.MatureContent.Id, 1900)
                    : string.Empty;

                return GamesEntityToDtoMapper.MapToMatureContentDto(pmc.MatureContent, contentLogoUrl);
            })
        );

        var rating = await _reviewRepository.GetProductApprovedReviewsCountAsync(createdAddon.Id);

        return GamesEntityToDtoMapper.MapToGameAddonDto(
            createdAddon,
            logoUrl,
            matureContentDtos.ToList(),
            rating
        );
    }

    /// <inheritdoc />
    public async Task<GameAddonDto?> UpdateAsync(GameAddonUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameAddonUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var existingAddon = await _gameAddonRepository.GetByIdAsync(request.Id);
        if (existingAddon == null)
        {
            throw new KeyNotFoundException($"GameAddon with ID {request.Id} not found.");
        }

        existingAddon.Title = request.Title;
        existingAddon.ShortDescription = request.ShortDescription;
        existingAddon.Description = request.Description;
        existingAddon.Developer = request.Developer;
        existingAddon.MinimalAge = (MinimalAge)request.MinimalAge;
        existingAddon.Platforms = request.Platforms;
        existingAddon.Price = request.Price;
        existingAddon.LogoUrl = request.LogoUrl;
        existingAddon.ReleaseDate = request.ReleaseDate;
        existingAddon.IsActive = request.IsActive;
        existingAddon.InterfaceLanguages = request.InterfaceLanguages;
        existingAddon.AudioLanguages = request.AudioLanguages;
        existingAddon.SubtitlesLanguages = request.SubtitlesLanguages;
        existingAddon.GameId = request.GameId;
        existingAddon.StorageUrl = request.StorageUrl;
        existingAddon.ModifiedAt = DateTime.UtcNow;

        var updatedAddon = await _gameAddonRepository.UpdateAsync(existingAddon);
        if (updatedAddon == null)
            return null;

        var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(updatedAddon.Id, 1900);

        var matureContentDtos = await Task.WhenAll(
            updatedAddon.ProductMatureContents.Select(async pmc =>
            {
                var contentLogoUrl = pmc.MatureContent.LogoUrl != null
                    ? await _fileManagerService.GetPresignedMatureContentImageUrlAsync(pmc.MatureContent.Id, 1900)
                    : string.Empty;

                return GamesEntityToDtoMapper.MapToMatureContentDto(pmc.MatureContent, contentLogoUrl);
            })
        );

        var rating = await _reviewRepository.GetProductApprovedReviewsCountAsync(updatedAddon.Id);

        return GamesEntityToDtoMapper.MapToGameAddonDto(
            updatedAddon,
            logoUrl,
            matureContentDtos.ToList(),
            rating
        );
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        await _gameAddonRepository.DeleteAsync(id);
    }
    
    /// <inheritdoc />
    public async Task<List<GameAddonShortDto>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameId));
        }
        
        var addons = await _gameAddonRepository.GetByGameIdAsync(gameId);
        
        var gameAddonsDtos = await Task.WhenAll(addons.Select(async a => 
            GamesEntityToDtoMapper.MapToGameAddonShortDto(
                a,
                await _fileManagerService.GetPresignedProductLogoUrlAsync(a.Id, 1900)
            )));

        return gameAddonsDtos.ToList();
    }
}