namespace Forja.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register your application services here
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<IUserProfileService, UserProfileService>();

        return services;
    }
}