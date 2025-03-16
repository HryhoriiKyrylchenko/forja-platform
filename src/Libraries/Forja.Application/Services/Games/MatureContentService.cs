namespace Forja.Application.Services.Games;

/// <summary>
/// Service for managing mature content data within the application.
/// Provides methods for retrieving, creating, updating, and deleting mature content records.
/// </summary>
public class MatureContentService : IMatureContentService
{
    private readonly IMatureContentRepository _matureContentRepository;

    public MatureContentService(IMatureContentRepository matureContentRepository)
    {
        _matureContentRepository = matureContentRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MatureContentDto>> GetAllAsync()
    {
        var matureContents = await _matureContentRepository.GetAllAsync();

        return matureContents.Select(GamesEntityToDtoMapper.MapToMatureContentDto);
    }

    /// <inheritdoc />
    public async Task<MatureContentDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var matureContent = await _matureContentRepository.GetByIdAsync(id);

        return matureContent == null ? null : GamesEntityToDtoMapper.MapToMatureContentDto(matureContent);
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

        return createdMatureContent == null ? null : GamesEntityToDtoMapper.MapToMatureContentDto(createdMatureContent);
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

        return updatedMatureContent == null ? null : GamesEntityToDtoMapper.MapToMatureContentDto(updatedMatureContent);
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