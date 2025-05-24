namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserAchievementRepository : IUserAchievementRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<UserAchievement> _userAchievements;
    
    public UserAchievementRepository(ForjaDbContext context)
    {
        _context = context;
        _userAchievements = context.Set<UserAchievement>();
    }
    
    /// <inheritdoc />
    public async Task<UserAchievement?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User achievement id cannot be empty.", nameof(id));
        }
        
        return await _userAchievements
            .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
            .Include(ua => ua.User)
            .FirstOrDefaultAsync(ua => ua.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserAchievement>> GetAllAsync()
    {
        return await _userAchievements
            .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
            .Include(ua => ua.User)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserAchievement>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _userAchievements
            .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
            .Where(ua => ua.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserAchievement>> GetNumByUserIdAsync(Guid userId, int num)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        return await _userAchievements
            .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
            .Where(ua => ua.UserId == userId)
            .OrderByDescending(ua => ua.AchievedAt)
            .Take(num) 
            .ToListAsync();
    }

    public async Task<List<Achievement>> GetAllAchievementsByUserIdAsync(Guid userId)
    {
        return await _userAchievements
            .Where(ua => ua.UserId == userId)
            .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
            .Select(ua => ua.Achievement)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserAchievement>> GetAllByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _userAchievements
            .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
            .Include(ua => ua.User)
            .Where(ua => ua.Achievement.GameId == gameId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserAchievement?> AddAsync(UserAchievement userAchievement)
    {
        if (!UserProfileModelValidator.ValidateUserAchievement(userAchievement))
        {
            throw new ArgumentException("User achievement is invalid.", nameof(userAchievement));
        }
        
        await _userAchievements.AddAsync(userAchievement);
        await _context.SaveChangesAsync();
        
        var addedUserAchievement = await GetByIdAsync(userAchievement.Id);
        return addedUserAchievement;
    }

    /// <inheritdoc />
    public async Task<UserAchievement?> UpdateAsync(UserAchievement userAchievement)
    {
        if (!UserProfileModelValidator.ValidateUserAchievement(userAchievement))
        {
            throw new ArgumentException("User achievement is invalid.", nameof(userAchievement));
        }
        
        _userAchievements.Update(userAchievement);
        await _context.SaveChangesAsync();
        
        var updatedUserAchievement = await GetByIdAsync(userAchievement.Id);
        return updatedUserAchievement;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            throw new ArgumentException("User achievement id cannot be empty.", nameof(userAchievementId));
        }
        
        var userAchievement = await GetByIdAsync(userAchievementId) 
                              ?? throw new ArgumentException("User achievement not found.", nameof(userAchievementId));

        _userAchievements.Remove(userAchievement);
        await _context.SaveChangesAsync();
    }
}