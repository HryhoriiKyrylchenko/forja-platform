namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserRepository : IUserRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<User> _users;
    
    public UserRepository(ForjaDbContext context)
    {
        _context = context;
        _users = context.Set<User>();
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id is required", nameof(id));
        }
        
        return await _users
            .Where(u => u.IsDeleted == false)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    /// <inheritdoc />
    public async Task<User?> GetDeletedByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id is required", nameof(id));
        }
        
        return await _users
            .Where(u => u.IsDeleted == true)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    /// <inheritdoc />
    public async Task<User?> GetByKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentException("User keycloak id is required", nameof(userKeycloakId));
        }
        
        return await _users
            .Where(u => u.IsDeleted == false)
            .FirstOrDefaultAsync(u => u.KeycloakUserId == userKeycloakId);
    }
    
    /// <inheritdoc />
    public async Task<User?> GetDeletedByKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentException("User keycloak id is required", nameof(userKeycloakId));
        }
        
        return await _users
            .Where(u => u.IsDeleted == true)
            .FirstOrDefaultAsync(u => u.KeycloakUserId == userKeycloakId);
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("User email is required", nameof(email));
        }
        
        return await _users
            .Where(u => u.IsDeleted == false)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _users
            .Where(u => u.IsDeleted == false)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllDeletedAsync()
    {
        return await _users
            .Where(u => u.IsDeleted == true)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(User user)
    {
        if (!ProjectModelValidator.ValidateUser(user))
        {
            throw new ArgumentException("User is not valid.", nameof(user));
        }
        
        await _users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(User user)
    {
        if (!ProjectModelValidator.ValidateUser(user))
        {
            throw new ArgumentException("User is not valid.", nameof(user));
        }
        
        _users.Update(user);
        await _context.SaveChangesAsync();

    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id is required", nameof(userId));
        }
        
        var user = await GetByIdAsync(userId) ?? throw new InvalidOperationException("No user found with the given id.");

        user.IsDeleted = true;
        user.ModifiedAt = DateTime.UtcNow;
        _users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task RestoreAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id is required", nameof(userId));
        }
           
        var user = await GetDeletedByIdAsync(userId) ?? throw new InvalidOperationException("No soft-deleted user found with the given id.");
           
        user.IsDeleted = false;
        user.ModifiedAt = DateTime.UtcNow;
        _users.Update(user);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required", nameof(username));
        }
        
        return await _users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
    }
}