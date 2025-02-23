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
                .ThenInclude(ga => ga.Game)
            .FirstOrDefaultAsync(ula => ula.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllByGameAsync(string gameTitle)
    {
        if (string.IsNullOrWhiteSpace(gameTitle))
        {
            throw new ArgumentException("Game title cannot be empty.", nameof(gameTitle));
        }
        
        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
                .ThenInclude(ga => ga.Game)
            .Where(ula => ula.UserLibraryGame.Game.Title == gameTitle)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllAsync()
    {
        if (_userLibraryAddons == null)
        {
            throw new Exception("User library addons not found.");
        }
        
        return await _userLibraryAddons
            .Where(ula => !ula.IsDeleted)
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
            .ThenInclude(ga => ga.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(UserLibraryAddon userLibraryAddon)
    {
        //TODO: Validate userLibraryAddon
        
        await _userLibraryAddons.AddAsync(userLibraryAddon);
    }

    /// <inheritdoc />
    public Task UpdateAsync(UserLibraryAddon userLibraryAddon)
    {
        //TODO: Validate userLibraryAddon
        
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
}