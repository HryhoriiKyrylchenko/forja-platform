using Forja.Application.Services.Authentication;

namespace Forja.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register your application services here
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddTransient<IClaimsTransformation, KeycloakRolesClaimsTransformer>();
        
        services.AddScoped<IGameService, GameService>();
        
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IGameSaveService, GameSaveService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserFollowerService, UserFollowerService>();
        services.AddScoped<IUserLibraryService, UserLibraryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserWishListService, UserWishListService>();

        return services;
    }
}