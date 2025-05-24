namespace Forja.Application.Services.Games;

/// <summary>
/// Service for managing genre-related operations in the application.
/// </summary>
public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var genres = await _genreRepository.GetAllAsync();

        return genres.Select(GamesEntityToDtoMapper.MapToGenreDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GenreDto>> GetAllDeletedAsync()
    {
        var deletedGenres = await _genreRepository.GetAllDeletedAsync();

        return deletedGenres.Select(GamesEntityToDtoMapper.MapToGenreDto);
    }

    /// <inheritdoc />
    public async Task<GenreDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var genre = await _genreRepository.GetByIdAsync(id);

        return genre == null ? null : GamesEntityToDtoMapper.MapToGenreDto(genre);
    }

    /// <inheritdoc />
    public async Task<GenreDto?> CreateAsync(GenreCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGenreCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newGenre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsDeleted = false
        };

        var createdGenre = await _genreRepository.AddAsync(newGenre);
        
        return createdGenre == null ? null : GamesEntityToDtoMapper.MapToGenreDto(createdGenre);
    }

    /// <inheritdoc />
    public async Task<GenreDto?> UpdateAsync(GenreUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGenreUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingGenre = await _genreRepository.GetByIdAsync(request.Id);
        if (existingGenre == null)
        {
            throw new KeyNotFoundException($"Genre with ID {request.Id} not found.");
        }

        existingGenre.Name = request.Name;
        existingGenre.IsDeleted = request.IsDeleted;

        var updatedGenre = await _genreRepository.UpdateAsync(existingGenre);

        return updatedGenre == null ? null : GamesEntityToDtoMapper.MapToGenreDto(updatedGenre);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var existingGenre = await _genreRepository.GetByIdAsync(id);
        if (existingGenre == null)
        {
            throw new KeyNotFoundException($"Genre with ID {id} not found.");
        }

        await _genreRepository.DeleteAsync(id);
    }
}