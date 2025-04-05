namespace Forja.Application.Services.Games;

/// <summary>
/// Service for managing mature content data within the application.
/// Provides methods for retrieving, creating, updating, and deleting mature content records.
/// </summary>
public class MatureContentService : IMatureContentService
{
    private readonly IMatureContentRepository _matureContentRepository;
    private readonly IFileManagerService _fileManagerService;

    public MatureContentService(IMatureContentRepository matureContentRepository,
        IFileManagerService fileManagerService)
    {
        _matureContentRepository = matureContentRepository;
        _fileManagerService = fileManagerService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MatureContentDto>> GetAllAsync()
    {
        var matureContents = await _matureContentRepository.GetAllAsync();

        return await Task.WhenAll(matureContents.Select(async mc =>
        {
            if (mc.LogoUrl != null)
                return GamesEntityToDtoMapper.MapToMatureContentDto(
                    mc,
                    await _fileManagerService.GetPresignedUrlAsync(mc.LogoUrl, 1900)
                );

            return GamesEntityToDtoMapper.MapToMatureContentDto(
                mc,
                string.Empty
            );
        }));
    }

    /// <inheritdoc />
    public async Task<MatureContentDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var matureContent = await _matureContentRepository.GetByIdAsync(id);

        return matureContent == null ? null : GamesEntityToDtoMapper.MapToMatureContentDto(
            matureContent,
            matureContent.LogoUrl == null ? string.Empty 
                : await _fileManagerService.GetPresignedUrlAsync(matureContent.LogoUrl, 1900)
            );
    }

    /// <inheritdoc />
    public async Task<MatureContentDto?> CreateAsync(MatureContentCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateMatureContentCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newMatureContent = new MatureContent
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            LogoUrl = request.LogoUrl
        };

        var createdMatureContent = await _matureContentRepository.AddAsync(newMatureContent);

        return createdMatureContent == null ? null : GamesEntityToDtoMapper.MapToMatureContentDto(
            createdMatureContent,
            createdMatureContent.LogoUrl == null? string.Empty 
                : await _fileManagerService.GetPresignedUrlAsync(createdMatureContent.LogoUrl, 1900));
    }

    /// <inheritdoc />
    public async Task<MatureContentDto?> UpdateAsync(MatureContentUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateMatureContentUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingMatureContent = await _matureContentRepository.GetByIdAsync(request.Id);
        if (existingMatureContent == null)
        {
            throw new KeyNotFoundException($"MatureContent with ID {request.Id} not found.");
        }

        existingMatureContent.Name = request.Name;
        existingMatureContent.Description = request.Description;
        existingMatureContent.LogoUrl = request.LogoUrl;

        var updatedMatureContent = await _matureContentRepository.UpdateAsync(existingMatureContent);

        return updatedMatureContent == null ? null : GamesEntityToDtoMapper.MapToMatureContentDto(
            updatedMatureContent,
            updatedMatureContent.LogoUrl == null ? string.Empty
                : await _fileManagerService.GetPresignedUrlAsync(updatedMatureContent.LogoUrl, 1900));
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var existingMatureContent = await _matureContentRepository.GetByIdAsync(id);
        if (existingMatureContent == null)
        {
            throw new KeyNotFoundException($"MatureContent with ID {id} not found.");
        }

        await _matureContentRepository.DeleteAsync(id);
    }
}