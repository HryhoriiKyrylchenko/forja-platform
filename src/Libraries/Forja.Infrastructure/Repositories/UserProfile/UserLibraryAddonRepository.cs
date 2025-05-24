namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserLibraryAddonRepository : IUserLibraryAddonRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<UserLibraryAddon> _userLibraryAddons;
    
    public UserLibraryAddonRepository(ForjaDbContext context)
    {
        _context = context;
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
    public async Task<UserLibraryAddon?> GetByGameIdAndUserIdAsync(Guid gameId, Guid userId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .FirstOrDefaultAsync(ula => ula.UserLibraryGame.GameId == gameId && ula.UserLibraryGame.UserId == userId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetByAddonIdAsync(Guid addonId)
    {
        if (addonId == Guid.Empty)
        {
            throw new ArgumentException("Addon id cannot be empty.", nameof(addonId));
        }
        
        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .ToListAsync();
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

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
                .ThenInclude(ga => ga.Game)
            .Where(ula => ula.UserLibraryGame.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
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
    public async Task<UserLibraryAddon?> AddAsync(UserLibraryAddon userLibraryAddon)
    {
        if (!UserProfileModelValidator.ValidateUserLibraryAddon(userLibraryAddon))
        {
            throw new ArgumentException("UserLibraryAddon is not valid.", nameof(userLibraryAddon));
        }
        
        await _userLibraryAddons.AddAsync(userLibraryAddon);
        await _context.SaveChangesAsync();

        var loadUserLibraryAddon = _userLibraryAddons.Include(ga => ga.GameAddon)
            .FirstOrDefaultAsync(ga => ga.Id == userLibraryAddon.Id);

        return userLibraryAddon;
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddon?> UpdateAsync(UserLibraryAddon userLibraryAddon)
    {
        if (!UserProfileModelValidator.ValidateUserLibraryAddon(userLibraryAddon))
        {
            throw new ArgumentException("UserLibraryAddon is not valid.", nameof(userLibraryAddon));
        }
        
        _userLibraryAddons.Update(userLibraryAddon);
        await _context.SaveChangesAsync();
        
        return userLibraryAddon;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon id cannot be empty.", nameof(userLibraryAddonId));
        }
        
        var userLibraryAddon = await GetByIdAsync(userLibraryAddonId) 
                               ?? throw new ArgumentException("User library addon not found.", nameof(userLibraryAddonId));

        userLibraryAddon.IsDeleted = true;
        _userLibraryAddons.Update(userLibraryAddon);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddon?> RestoreAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon id cannot be empty.", nameof(userLibraryAddonId));
        }
        
        var deletedUserLibraryAddon = await GetDeletedByIdAsync(userLibraryAddonId) 
                                      ?? throw new ArgumentException("User library addon not found.", nameof(userLibraryAddonId));

        if (!deletedUserLibraryAddon.IsDeleted)
        {
            return deletedUserLibraryAddon;
        }
        
        deletedUserLibraryAddon.IsDeleted = false;
        _userLibraryAddons.Update(deletedUserLibraryAddon);
        await _context.SaveChangesAsync();
        
        return deletedUserLibraryAddon;
    }

    /// <inheritdoc />
    public async Task<int> GetAllAddonsCountByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .CountAsync(ula => ula.UserLibraryGame.UserId == userId);
    }
}