namespace Forja.Infrastructure.Repositories;

public class UserProfileUnitOfWork : IUserProfileUnitOfWork
{
    private readonly ForjaDbContext _context;

    public IUserRepository Users { get; }
    public IAchievementRepository Achievements { get; }
    public IReviewRepository Reviews { get; }
    public IUserAchievementRepository UserAchievements { get; }
    public IUserLibraryGameRepository UserLibraryGames { get; }
    public IUserLibraryAddonRepository UserLibraryAddons { get; }

    public UserProfileUnitOfWork(
        ForjaDbContext context, 
        IUserRepository userRepository,
        IAchievementRepository achievementRepository,
        IReviewRepository reviewRepository,
        IUserAchievementRepository userAchievementRepository,
        IUserLibraryGameRepository userLibraryGameRepository,
        IUserLibraryAddonRepository userLibraryAddonRepository)
    {
        _context = context;

        Users = userRepository;                  
        Achievements = achievementRepository;
        Reviews = reviewRepository;
        UserAchievements = userAchievementRepository;
        UserLibraryGames = userLibraryGameRepository;
        UserLibraryAddons = userLibraryAddonRepository;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}