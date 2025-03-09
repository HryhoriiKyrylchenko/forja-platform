namespace Forja.Infrastructure.Repositories.UserProfile;

public class AchievementRepository : IAchievementRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Achievement> _achievements;
    
    public AchievementRepository(ForjaDbContext context)
    {
        _context = context;
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
            .Include(a => a.Game)
            .FirstOrDefaultAsync(a => a.Id == id);
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
            .Include(a => a.Game)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllDeletedByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _achievements
            .Where(a => a.IsDeleted)
            .Where(a => a.GameId == gameId)
            .Include(a => a.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllAsync()
    {
        return await _achievements
            .Where(a => !a.IsDeleted)
            .Include(a => a.Game)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Achievement>> GetAllDeletedAsync()
    {
        return await _achievements
            .Where(a => a.IsDeleted)
            .Include(a => a.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Achievement achievement)
    {
        if (!UserProfileModelValidator.ValidateAchievement(achievement))
        {
            throw new ArgumentException("Achievement is not valid.", nameof(achievement));
        }
        
        await _achievements.AddAsync(achievement);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Achievement achievement)
    {
        if (!UserProfileModelValidator.ValidateAchievement(achievement))
        {
            throw new ArgumentException("Achievement is not valid.", nameof(achievement));
        }
        
        _achievements.Update(achievement);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement id cannot be empty.", nameof(achievementId));
        }
        
        var achievement = await GetByIdAsync(achievementId) 
                          ?? throw new ArgumentException("Achievement not found.", nameof(achievementId));
        
        achievement.IsDeleted = true;
        _achievements.Update(achievement);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<Achievement> RestoreAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement id cannot be empty.", nameof(achievementId));
        }
        
        var achievement = await GetByIdAsync(achievementId) ?? throw new ArgumentException("Achievement not found.", nameof(achievementId));
        
        if (!achievement.IsDeleted)
        {
            return achievement;
        }
        
        achievement.IsDeleted = false;
        _achievements.Update(achievement);
        await _context.SaveChangesAsync();
        
        return achievement;
    }
}