using Forja.Infrastructure.Validators;

namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserLibraryAddonRepository : IUserLibraryAddonRepository
{
    private readonly DbSet<UserLibraryAddon> _userLibraryAddons;
    
    public UserLibraryAddonRepository(ForjaDbContext context)
    {
        _userLibraryAddons = context.Set<UserLibraryAddon>();
    }
    
    /// <inheritdoc />
    public async Task<UserLibraryAddon?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User library addon id cannot be empty.", nameof(id));
        }
        
        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
            .FirstOrDefaultAsync(ula => ula.Id == id);
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddon?> GetDeletedByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User library addon id cannot be empty.", nameof(id));
        }
        
        return await _userLibraryAddons
            .Where(ula => ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
            .FirstOrDefaultAsync(ula => ula.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
                .ThenInclude(ga => ga.Game)
            .Where(ula => ula.UserLibraryGame.Game.Id == gameId)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserLibraryAddon>> GetAllDeletedByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _userLibraryAddons
            .Where(ula => ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
                .ThenInclude(ga => ga.Game)
            .Where(ula => ula.UserLibraryGame.Game.Id == gameId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllAsync()
    {
        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllDeletedAsync()
    {
        return await _userLibraryAddons
            .Where(ula => ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(UserLibraryAddon userLibraryAddon)
    {
        if (!ProjectModelValidator.ValidateUserLibraryAddon(userLibraryAddon))
        {
            throw new ArgumentException("UserLibraryAddon is not valid.", nameof(userLibraryAddon));
        }
        
        await _userLibraryAddons.AddAsync(userLibraryAddon);
    }

    /// <inheritdoc />
    public Task UpdateAsync(UserLibraryAddon userLibraryAddon)
    {
        if (!ProjectModelValidator.ValidateUserLibraryAddon(userLibraryAddon))
        {
            throw new ArgumentException("UserLibraryAddon is not valid.", nameof(userLibraryAddon));
        }
        
        _userLibraryAddons.Update(userLibraryAddon);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon id cannot be empty.", nameof(userLibraryAddonId));
        }
        
        var userLibraryAddon = await GetByIdAsync(userLibraryAddonId);
        if (userLibraryAddon != null)
        {
            userLibraryAddon.IsDeleted = true;
            _userLibraryAddons.Update(userLibraryAddon);
        }
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddon> RestoreAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon id cannot be empty.", nameof(userLibraryAddonId));
        }
        
        var deletedUserLibraryAddon = await GetDeletedByIdAsync(userLibraryAddonId);
        if (deletedUserLibraryAddon == null)
        {
            throw new ArgumentException("User library game not found.", nameof(userLibraryAddonId));
        }

        if (!deletedUserLibraryAddon.IsDeleted)
        {
            return deletedUserLibraryAddon;
        }
        
        deletedUserLibraryAddon.IsDeleted = false;
        _userLibraryAddons.Update(deletedUserLibraryAddon);
        
        return deletedUserLibraryAddon;
    }
}