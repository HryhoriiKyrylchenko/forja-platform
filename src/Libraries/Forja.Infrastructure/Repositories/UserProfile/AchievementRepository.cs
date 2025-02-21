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
        return await _achievements
            .Include(a => a.Game)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllByUserIdAsync(Guid userId)
    {
        return await _achievements
            .Include(a => a.Game)
            .Include(a => a.UserAchievements)
                .ThenInclude(ua => ua.User)
            .Where(a => a.UserAchievements.Any(ua => ua.User.Id == userId))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllByGameIdAsync(Guid gameId)
    {
        return await _achievements
            .Include(a => a.Game)
            .Where(a => a.GameId == gameId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllAsync()
    {
        return await _achievements
            .Include(a => a.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Achievement achievement)
    {
        await _achievements.AddAsync(achievement);
    }

    /// <inheritdoc />
    public Task UpdateAsync(Achievement achievement)
    {
        _achievements.Update(achievement);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid achievementId)
    {
        var achievement = await GetByIdAsync(achievementId);
        if (achievement != null)
        {
            achievement.IsDeleted = true;
            _achievements.Update(achievement);
        }
    }
}