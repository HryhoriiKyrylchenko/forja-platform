using Forja.Application.Services.Authentication;

namespace Forja.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register your application services here
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<ITokenService, TokenService>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IUserAchievementService, UserAchievementService>();
        services.AddScoped<IUserLibraryService, UserLibraryService>();
        
        services.AddScoped<IGameService, GameService>();

        return services;
    }
}