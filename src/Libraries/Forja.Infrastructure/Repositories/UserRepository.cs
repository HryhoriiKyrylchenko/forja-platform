namespace Forja.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ForjaDbContext _dbContext;
    
    public UserRepository(ForjaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));
        
        return await _dbContext.Users
            .AnyAsync(u => u.Username.ToLower() == username.ToLower());
    }
}