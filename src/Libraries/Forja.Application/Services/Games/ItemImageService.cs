namespace Forja.Application.Services.Games;

/// <summary>
/// Service responsible for managing the operations related to ItemImage entities.
/// </summary>
public class ItemImageService : IItemImageService
{
    private readonly IItemImageRepository _itemImageRepository;
    private readonly IFileManagerService _fileManagerService;

    public ItemImageService(IItemImageRepository itemImageRepository,
        IFileManagerService fileManagerService)
    {
        _itemImageRepository = itemImageRepository;
        _fileManagerService = fileManagerService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemImageDto>> GetAllAsync()
    {
        var itemImages = await _itemImageRepository.GetAllAsync();

        return await Task.WhenAll(itemImages.Select(async i =>GamesEntityToDtoMapper.MapToItemImageDto(
            i,
            await _fileManagerService.GetPresignedUrlAsync(i.ImageUrl, 1900)
        )));
    }

    /// <inheritdoc />
    public async Task<ItemImageDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var itemImage = await _itemImageRepository.GetByIdAsync(id);

        return itemImage == null ? null : GamesEntityToDtoMapper.MapToItemImageDto(
                                            itemImage,
                                            await _fileManagerService.GetPresignedUrlAsync(itemImage.ImageUrl, 1900));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemImageDto>> GetAllWithProductImagesAsync()
    {
        var itemImagesWithProductImages = await _itemImageRepository.GetAllWithProductImagesAsync();

        return await Task.WhenAll(itemImagesWithProductImages.Select(async i =>GamesEntityToDtoMapper.MapToItemImageDto(
            i,
            await _fileManagerService.GetPresignedUrlAsync(i.ImageUrl, 1900)
        )));
    }
    
    /// <inheritdoc />
    public async Task<ItemImageDto?> CreateAsync(ItemImageCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateItemImageCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newItemImage = new ItemImage
        {
            Id = Guid.NewGuid(),
            ImageUrl = request.ImageUrl,
            ImageAlt = request.ImageAlt
        };

        var createdItemImage = await _itemImageRepository.AddAsync(newItemImage);

        return createdItemImage == null ? null : GamesEntityToDtoMapper.MapToItemImageDto(
            createdItemImage,
            await _fileManagerService.GetPresignedUrlAsync(createdItemImage.ImageUrl, 1900));
    }

    /// <inheritdoc />
    public async Task<ItemImageDto?> UpdateAsync(ItemImageUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateItemImageUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingItemImage = await _itemImageRepository.GetByIdAsync(request.Id);
        if (existingItemImage == null)
        {
            throw new KeyNotFoundException($"ItemImage with ID {request.Id} not found.");
        }

        existingItemImage.ImageUrl = request.ImageUrl;
        existingItemImage.ImageAlt = request.ImageAlt;

        var updatedItemImage = await _itemImageRepository.UpdateAsync(existingItemImage);

        return updatedItemImage == null ? null : GamesEntityToDtoMapper.MapToItemImageDto(
            updatedItemImage,
            await _fileManagerService.GetPresignedUrlAsync(updatedItemImage.ImageUrl, 1900));
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var existingItemImage = await _itemImageRepository.GetByIdAsync(id);
        if (existingItemImage == null)
        {
            throw new KeyNotFoundException($"ItemImage with ID {id} not found.");
        }

        await _itemImageRepository.DeleteAsync(id);
    }
}