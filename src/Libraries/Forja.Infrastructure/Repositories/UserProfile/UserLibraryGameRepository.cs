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
    public async Task<UserLibraryGame?> GetDeletedByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User library game id cannot be empty.", nameof(id));
        }
        
        return await _userLibraryGames
            .Where(ulg => ulg.IsDeleted)
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
    public async Task<IEnumerable<UserLibraryGame>> GetAllDeletedAsync()
    {
        return await _userLibraryGames
            .Where(ulg => ulg.IsDeleted)
            .Include(ulg => ulg.User)
            .Include(ulg => ulg.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryGame>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _userLibraryGames
            .Where(ulg => ulg.UserId == userId)
            .Where(ulg => !ulg.IsDeleted)
            .Include(ulg => ulg.User)
            .Include(ulg => ulg.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryGame>> GetAllDeletedByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _userLibraryGames
            .Where(ulg => ulg.UserId == userId)
            .Where(ulg => ulg.IsDeleted)
            .Include(ulg => ulg.User)
            .Include(ulg => ulg.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(UserLibraryGame userLibraryGame)
    {
        if (userLibraryGame == null)
        {
            throw new ArgumentNullException(nameof(userLibraryGame));
        }
        
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

    /// <inheritdoc />
    public async Task<UserLibraryGame> RestoreAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User library game id cannot be empty.", nameof(id));
        }
        
        var deletedUserLibraryGame = await GetDeletedByIdAsync(id);
        if (deletedUserLibraryGame == null)
        {
            throw new ArgumentException("User library game not found.", nameof(id));
        }

        if (!deletedUserLibraryGame.IsDeleted)
        {
            return deletedUserLibraryGame;
        }
        
        deletedUserLibraryGame.IsDeleted = false;
        _userLibraryGames.Update(deletedUserLibraryGame);
        
        return deletedUserLibraryGame;
    }
}