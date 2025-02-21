namespace Forja.Infrastructure.Repositories;

public class UserProfileUnitOfWork : IUserProfileUnitOfWork
{
    private readonly ForjaDbContext _context;
    
    private IUserRepository? _userRepository;
    private IAchievementRepository? _achievementRepository;
    private IReviewRepository? _reviewRepository;
    private IUserAchievementRepository? _userAchievementRepository;
    private IUserLibraryGameRepository? _userLibraryGameRepository;
    private IUserLibraryAddonRepository? _userLibraryAddonRepository;
    
    public UserProfileUnitOfWork(ForjaDbContext context)
    {
        _context = context;
    }
    
    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    public IAchievementRepository Achievements => _achievementRepository ??= new AchievementRepository(_context);
    public IReviewRepository Reviews => _reviewRepository ??= new ReviewRepository(_context);
    public IUserAchievementRepository UserAchievements => _userAchievementRepository ??= new UserAchievementRepository(_context);
    public IUserLibraryGameRepository UserLibraryGames => _userLibraryGameRepository ??= new UserLibraryGameRepository(_context);
    public IUserLibraryAddonRepository UserLibraryAddons => _userLibraryAddonRepository ??= new UserLibraryAddonRepository(_context);
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}