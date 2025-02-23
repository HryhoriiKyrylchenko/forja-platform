namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserLibraryGameRepository : IUserLibraryGameRepository
{
    private readonly DbSet<UserLibraryGame> _userLibraryGames;
    
    public UserLibraryGameRepository(ForjaDbContext context)
    {
        _userLibraryGames = context.Set<UserLibraryGame>();
    }
    
    /// <inheritdoc />
    public async Task<UserLibraryGame?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User library game id cannot be empty.", nameof(id));
        }
        
        return await _userLibraryGames
            .Where(ulg => !ulg.IsDeleted)
            .Include(ulg => ulg.User)
            .Include(ulg => ulg.Game)
            .FirstOrDefaultAsync(ulg => ulg.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryGame>> GetAllAsync()
    {
        return await _userLibraryGames
            .Where(ulg => !ulg.IsDeleted)
            .Include(ulg => ulg.User)
            .Include(ulg => ulg.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(UserLibraryGame userLibraryGame)
    {
        //TODO: Validate userLibraryGame
        
        await _userLibraryGames.AddAsync(userLibraryGame);
    }

    /// <inheritdoc />
    public Task UpdateAsync(UserLibraryGame userLibraryGame)
    {
        //TODO: Validate userLibraryGame
        
        _userLibraryGames.Update(userLibraryGame);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game id cannot be empty.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await GetByIdAsync(userLibraryGameId);
        if (userLibraryGame != null)
        {
            userLibraryGame.IsDeleted = true;
            _userLibraryGames.Update(userLibraryGame);
        }
    }
}