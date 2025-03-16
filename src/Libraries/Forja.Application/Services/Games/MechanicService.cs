namespace Forja.Application.Services.Games;

/// <summary>
/// Service class responsible for handling mechanics data and operations.
/// </summary>
public class MechanicService : IMechanicService
{
    private readonly IMechanicRepository _mechanicRepository;

    public MechanicService(IMechanicRepository mechanicRepository)
    {
        _mechanicRepository = mechanicRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MechanicDto>> GetAllAsync()
    {
        var mechanics = await _mechanicRepository.GetAllAsync();

        return mechanics.Select(GamesEntityToDtoMapper.MapToMechanicDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MechanicDto>> GetAllDeletedAsync()
    {
        var deletedMechanics = await _mechanicRepository.GetAllDeletedAsync();

        return deletedMechanics.Select(GamesEntityToDtoMapper.MapToMechanicDto);
    }

    /// <inheritdoc />
    public async Task<MechanicDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        var mechanic = await _mechanicRepository.GetByIdAsync(id);

        return mechanic == null ? null : GamesEntityToDtoMapper.MapToMechanicDto(mechanic);
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

        return createdMechanic == null ? null : GamesEntityToDtoMapper.MapToMechanicDto(createdMechanic);
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
        existingMechanic.LogoUrl = request.LogoUrl;
        existingMechanic.IsDeleted = request.IsDeleted;

        var updatedMechanic = await _mechanicRepository.UpdateAsync(existingMechanic);

        return updatedMechanic == null ? null : GamesEntityToDtoMapper.MapToMechanicDto(updatedMechanic);
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