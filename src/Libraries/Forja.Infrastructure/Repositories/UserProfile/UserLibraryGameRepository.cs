namespace Forja.Infrastructure.Repositories.UserProfile;

public class UserLibraryGameRepository : IUserLibraryGameRepository
{
    private readonly DbSet<UserLibraryGame> _userLibraryGames;
    
    public UserLibraryGameRepository(ForjaDbContext context)
    {
        _userLibraryGames = context.Set<UserLibraryGame>();
    }
    
    /// <inheritdoc />
    public async Task<UserLibraryGame?> GetByIdAsync(Guid id)
    {
        return await _userLibraryGames
            .Include(ulg => ulg.User)
            .Include(ulg => ulg.Game)
            .FirstOrDefaultAsync(ulg => ulg.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserLibraryGame>> GetAllAsync()
    {
        return await _userLibraryGames
            .Include(ulg => ulg.User)
            .Include(ulg => ulg.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(UserLibraryGame userLibraryGame)
    {
        await _userLibraryGames.AddAsync(userLibraryGame);
    }

    /// <inheritdoc />
    public Task UpdateAsync(UserLibraryGame userLibraryGame)
    {
        _userLibraryGames.Update(userLibraryGame);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userLibraryGameId)
    {
        var userLibraryGame = await GetByIdAsync(userLibraryGameId);
        if (userLibraryGame != null)
        {
            userLibraryGame.IsDeleted = true;
            _userLibraryGames.Update(userLibraryGame);
        }
    }
}