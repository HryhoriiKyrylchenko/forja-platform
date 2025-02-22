namespace Forja.Infrastructure.Repositories.UserProfile;

public class AchievementRepository : IAchievementRepository
{
    private readonly DbSet<Achievement> _achievements;
    
    public AchievementRepository(ForjaDbContext context)
    {
        _achievements = context.Set<Achievement>();
    }
    
    /// <inheritdoc />
    public async Task<Achievement?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Achievement id cannot be empty.", nameof(id));
        }
        
        return await _achievements
            .Where(a => !a.IsDeleted)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _achievements
            .Where(a => !a.IsDeleted)
            .Where(a => a.UserAchievements.Any(ua => ua.User.Id == userId))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _achievements
            .Where(a => !a.IsDeleted)
            .Where(a => a.GameId == gameId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllAsync()
    {
        return await _achievements
            .Where(a => !a.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Achievement achievement)
    {
        //TODO: Validate achievement
        
        await _achievements.AddAsync(achievement);
    }

    /// <inheritdoc />
    public Task UpdateAsync(Achievement achievement)
    {
        //TODO: Validate achievement
        
        _achievements.Update(achievement);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement id cannot be empty.", nameof(achievementId));
        }
        
        var achievement = await GetByIdAsync(achievementId);
        if (achievement != null)
        {
            achievement.IsDeleted = true;
            _achievements.Update(achievement);
        }
    }
}