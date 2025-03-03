namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserLibraryGameRepository : IUserLibraryGameRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<UserLibraryGame> _userLibraryGames;
    
    public UserLibraryGameRepository(ForjaDbContext context)
    {
        _context = context;
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
        if (!ProjectModelValidator.ValidateUserLibraryGame(userLibraryGame))
        {
            throw new ArgumentException("User library game is invalid.", nameof(userLibraryGame));
        }
        
        await _userLibraryGames.AddAsync(userLibraryGame);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserLibraryGame userLibraryGame)
    {
        if (!ProjectModelValidator.ValidateUserLibraryGame(userLibraryGame))
        {
            throw new ArgumentException("User library game is invalid.", nameof(userLibraryGame));
        }
        
        _userLibraryGames.Update(userLibraryGame);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game id cannot be empty.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await GetByIdAsync(userLibraryGameId) ?? throw new ArgumentException("User library game not found.", nameof(userLibraryGameId));
            
        userLibraryGame.IsDeleted = true;
        _userLibraryGames.Update(userLibraryGame);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<UserLibraryGame> RestoreAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User library game id cannot be empty.", nameof(id));
        }
        
        var deletedUserLibraryGame = await GetDeletedByIdAsync(id) ?? throw new ArgumentException("User library game not found.", nameof(id));

        if (!deletedUserLibraryGame.IsDeleted)
        {
            return deletedUserLibraryGame;
        }
        
        deletedUserLibraryGame.IsDeleted = false;
        _userLibraryGames.Update(deletedUserLibraryGame);
        await _context.SaveChangesAsync();
        
        return deletedUserLibraryGame;
    }
}