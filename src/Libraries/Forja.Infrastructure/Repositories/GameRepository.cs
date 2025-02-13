namespace Forja.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ForjaDbContext _context;
    
    public GameRepository(ForjaDbContext context)
    {
        _context = context;
    }
    
    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _context.Games.FindAsync(id);
    }
    
    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _context.Games.ToListAsync();
    }
    
    public async Task AddAsync(Game game)
    {
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
    }
    
}