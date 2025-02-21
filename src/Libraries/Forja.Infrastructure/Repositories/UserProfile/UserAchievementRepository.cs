namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserAchievementRepository : IUserAchievementRepository
{
    private readonly DbSet<UserAchievement> _userAchievements;
    
    public UserAchievementRepository(ForjaDbContext context)
    {
        _userAchievements = context.Set<UserAchievement>();
    }
    
    /// <inheritdoc />
    public async Task<UserAchievement?> GetByIdAsync(Guid id)
    {
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

    /// <inheritdoc />
    public async Task AddAsync(UserAchievement userAchievement)
    {
        await _userAchievements.AddAsync(userAchievement);
    }

    /// <inheritdoc />
    public Task UpdateAsync(UserAchievement userAchievement)
    {
        _userAchievements.Update(userAchievement);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userAchievementId)
    {
        var userAchievement = await GetByIdAsync(userAchievementId) 
                              ?? throw new ArgumentException("User achievement not found.", nameof(userAchievementId));

        _userAchievements.Remove(userAchievement);
        await Task.CompletedTask;
    }
}