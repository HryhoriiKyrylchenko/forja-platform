namespace Forja.Application.Services.Games;

/// <summary>
/// Service class responsible for handling mechanics data and operations.
/// </summary>
public class MechanicService : IMechanicService
{
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IFileManagerService _fileManagerService;

    public MechanicService(IMechanicRepository mechanicRepository,
        IFileManagerService fileManagerService)
    {
        _mechanicRepository = mechanicRepository;
        _fileManagerService = fileManagerService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MechanicDto>> GetAllAsync()
    {
        var mechanics = await _mechanicRepository.GetAllAsync();

        return await Task.WhenAll(mechanics.Select(async m => GamesEntityToDtoMapper.MapToMechanicDto(
            m,
            m.LogoUrl == null ? string.Empty 
                    : await _fileManagerService.GetPresignedUrlAsync(m.LogoUrl, 1900)
            )));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MechanicDto>> GetAllDeletedAsync()
    {
        var deletedMechanics = await _mechanicRepository.GetAllDeletedAsync();

        return await Task.WhenAll(deletedMechanics.Select(async m => GamesEntityToDtoMapper.MapToMechanicDto(
            m,
            m.LogoUrl == null ? string.Empty 
                : await _fileManagerService.GetPresignedUrlAsync(m.LogoUrl, 1900)
        )));
    }

    /// <inheritdoc />
    public async Task<MechanicDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var mechanic = await _mechanicRepository.GetByIdAsync(id);

        return mechanic == null ? null : GamesEntityToDtoMapper.MapToMechanicDto(
            mechanic,
            mechanic.LogoUrl == null ? string.Empty
                : await _fileManagerService.GetPresignedUrlAsync(mechanic.LogoUrl, 1900));
    }

    /// <inheritdoc />
    public async Task<MechanicDto?> CreateAsync(MechanicCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateMechanicCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var newMechanic = new Mechanic
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            LogoUrl = request.LogoUrl,
            IsDeleted = false
        };

        var createdMechanic = await _mechanicRepository.AddAsync(newMechanic);

        return createdMechanic == null ? null : GamesEntityToDtoMapper.MapToMechanicDto(
            createdMechanic,
            createdMechanic.LogoUrl == null ? string.Empty
                : await _fileManagerService.GetPresignedUrlAsync(createdMechanic.LogoUrl, 1900));
    }

    /// <inheritdoc />
    public async Task<MechanicDto?> UpdateAsync(MechanicUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateMechanicUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }

        var existingMechanic = await _mechanicRepository.GetByIdAsync(request.Id);
        if (existingMechanic == null)
        {
            throw new KeyNotFoundException($"Mechanic with ID {request.Id} not found.");
        }

        existingMechanic.Name = request.Name;
        existingMechanic.Description = request.Description;
        existingMechanic.IsDeleted = request.IsDeleted;

        var updatedMechanic = await _mechanicRepository.UpdateAsync(existingMechanic);

        return updatedMechanic == null ? null : GamesEntityToDtoMapper.MapToMechanicDto(
            updatedMechanic,
            updatedMechanic.LogoUrl == null ? string.Empty
                : await _fileManagerService.GetPresignedUrlAsync(updatedMechanic.LogoUrl, 1900));
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var existingMechanic = await _mechanicRepository.GetByIdAsync(id);
        if (existingMechanic == null)
        {
            throw new KeyNotFoundException($"Mechanic with ID {id} not found.");
        }

        await _mechanicRepository.DeleteAsync(id);
    }
}