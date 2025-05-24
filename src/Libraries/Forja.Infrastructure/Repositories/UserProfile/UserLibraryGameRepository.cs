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
    public async Task<UserLibraryGame?> GetByGameIdAndUserIdAsync(Guid gameId, Guid userId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _userLibraryGames
            .Where(ulg => !ulg.IsDeleted)
            .FirstOrDefaultAsync(ulg => ulg.UserId == userId && ulg.GameId == gameId);
    }

    /// <inheritdoc />
    public async Task<UserLibraryGame?> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _userLibraryGames
            .Where(ulg => !ulg.IsDeleted)
            .FirstOrDefaultAsync(ulg => ulg.GameId == gameId);
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
                .ThenInclude(u => u.UserAchievements)
                    .ThenInclude(ua => ua.Achievement)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.Achievements)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.ProductGenres)
                    .ThenInclude(pg => pg.Genre)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.ProductMatureContents)
                    .ThenInclude(mc => mc.MatureContent)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.GameTags)
                    .ThenInclude(t => t.Tag)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.GameMechanics)
                    .ThenInclude(gm => gm.Mechanic)
            .Include(ulg => ulg.PurchasedAddons)
                .ThenInclude(a => a.GameAddon)
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
                .ThenInclude(u => u.UserAchievements)
                    .ThenInclude(ua => ua.Achievement)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.Achievements)
            .Include(ulg => ulg.PurchasedAddons)
                .ThenInclude(a => a.GameAddon)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserLibraryGame?> AddAsync(UserLibraryGame userLibraryGame)
    {
        if (!UserProfileModelValidator.ValidateUserLibraryGame(userLibraryGame))
        {
            throw new ArgumentException("User library game is invalid.", nameof(userLibraryGame));
        }
        
        await _userLibraryGames.AddAsync(userLibraryGame);
        await _context.SaveChangesAsync();

        return await _userLibraryGames
            .Include(g => g.Game)
            .FirstOrDefaultAsync(g => g.Id == userLibraryGame.Id);
    }

    /// <inheritdoc />
    public async Task<UserLibraryGame?> UpdateAsync(UserLibraryGame userLibraryGame)
    {
        if (!UserProfileModelValidator.ValidateUserLibraryGame(userLibraryGame))
        {
            throw new ArgumentException("User library game is invalid.", nameof(userLibraryGame));
        }
        
        _userLibraryGames.Update(userLibraryGame);
        await _context.SaveChangesAsync();
        
        return userLibraryGame;
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
    public async Task<UserLibraryGame?> RestoreAsync(Guid id)
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

    /// <inheritdoc />
    public async Task<int> GetAllGamesCountByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        return await _userLibraryGames
            .Where(ulg => ulg.UserId == userId &&
                   ulg.IsDeleted != true)
            .CountAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryGame>> GetAllForLauncher(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _userLibraryGames
            .Where(ulg => ulg.UserId == userId &&
                   !ulg.IsDeleted)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.ProductVersions)
                    .ThenInclude(v => v.Files)
            .Include(ulg => ulg.Game)
                .ThenInclude(g => g.GamePatches)
            .Include(ulg => ulg.PurchasedAddons)
                .ThenInclude(a => a.GameAddon)
                    .ThenInclude(g => g.ProductVersions)
                        .ThenInclude(a => a.Files)
            .ToListAsync();
    }
}