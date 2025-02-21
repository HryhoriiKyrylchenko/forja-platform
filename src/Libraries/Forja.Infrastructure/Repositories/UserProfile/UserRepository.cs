namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;
    
    public UserRepository(ForjaDbContext context)
    {
        _users = context.Set<User>();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _users
            .Include(u => u.UserAchievements)
            .Include(u => u.Reviews)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _users
            .Include(u => u.UserAchievements)
            .Include(u => u.Reviews)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _users
            .Include(u => u.UserAchievements)
                .ThenInclude(ua => ua.Achievement)
            .Include(u => u.Reviews)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(User user)
    {
        await _users.AddAsync(user);
    }

    /// <inheritdoc />
    public Task UpdateAsync(User user)
    {
        _users.Update(user);
        return Task.CompletedTask;

    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId)
    {
        var user = await GetByIdAsync(userId);
        if (user != null)
        {
            user.IsDeleted = true;
            user.ModifiedAt = DateTime.UtcNow;
            _users.Update(user);
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));
        
        return await _users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
    }
}