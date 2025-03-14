namespace Forja.Application.Services.Games;

/// <summary>
/// Provides operations for managing game-related tags.
/// </summary>
public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TagDto>> GetAllAsync()
    {
        var tags = await _tagRepository.GetAllAsync();

        return tags.Select(GamesEntityToDtoMapper.MapToTagDto);
    }

    /// <inheritdoc />
    public async Task<TagDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var tag = await _tagRepository.GetByIdAsync(id);

        return tag == null ? null : GamesEntityToDtoMapper.MapToTagDto(tag);
    }

    /// <inheritdoc />
    public async Task<TagDto?> CreateAsync(TagCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateTagCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newTag = new Tag
        {
            Id = Guid.NewGuid(),
            Title = request.Title
        };

        var createdTag = await _tagRepository.AddAsync(newTag);

        return createdTag == null ? null : GamesEntityToDtoMapper.MapToTagDto(createdTag);
    }

    /// <inheritdoc />
    public async Task<TagDto?> UpdateAsync(TagUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateTagUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingTag = await _tagRepository.GetByIdAsync(request.Id);
        if (existingTag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {request.Id} not found.");
        }

        existingTag.Title = request.Title;

        var updatedTag = await _tagRepository.UpdateAsync(existingTag);

        return updatedTag == null ? null : GamesEntityToDtoMapper.MapToTagDto(updatedTag);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var existingTag = await _tagRepository.GetByIdAsync(id);
        if (existingTag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {id} not found.");
        }

        await _tagRepository.DeleteAsync(id);
    }
}