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
        return await _userLibraryAddons
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
                .ThenInclude(ga => ga.Game)
            .FirstOrDefaultAsync(ula => ula.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllByGameAsync(string gameTitle)
    {
        return await _userLibraryAddons
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
                .ThenInclude(ga => ga.Game)
            .Where(ula => ula.UserLibraryGame.Game.Title == gameTitle)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryAddon>> GetAllAsync()
    {
        return await _userLibraryAddons
            .Include(ula => ula.UserLibraryGame)
            .Include(ula => ula.GameAddon)
            .ThenInclude(ga => ga.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(UserLibraryAddon userLibraryAddon)
    {
        await _userLibraryAddons.AddAsync(userLibraryAddon);
    }

    /// <inheritdoc />
    public Task UpdateAsync(UserLibraryAddon userLibraryAddon)
    {
        _userLibraryAddons.Update(userLibraryAddon);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userLibraryAddonId)
    {
        var userLibraryAddon = await GetByIdAsync(userLibraryAddonId);
        if (userLibraryAddon != null)
        {
            userLibraryAddon.IsDeleted = true;
            _userLibraryAddons.Update(userLibraryAddon);
        }
    }
}