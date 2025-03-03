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
            .Include(ua => ua.User)
            .FirstOrDefaultAsync(ua => ua.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserAchievement>> GetAllAsync()
    {
        return await _userAchievements
            .Include(ua => ua.Achievement)
            .Include(ua => ua.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserAchievement>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _userAchievements
            .Include(ua => ua.Achievement)
            .Include(ua => ua.User)
            .Where(ua => ua.UserId == userId)
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
    public async Task AddAsync(UserAchievement userAchievement)
    {
        if (!ProjectModelValidator.ValidateUserAchievement(userAchievement))
        {
            throw new ArgumentException("User achievement is invalid.", nameof(userAchievement));
        }
        
        await _userAchievements.AddAsync(userAchievement);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserAchievement userAchievement)
    {
        if (!ProjectModelValidator.ValidateUserAchievement(userAchievement))
        {
            throw new ArgumentException("User achievement is invalid.", nameof(userAchievement));
        }
        
        _userAchievements.Update(userAchievement);
        await _context.SaveChangesAsync();
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