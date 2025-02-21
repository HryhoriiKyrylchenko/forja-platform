namespace Forja.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register your application services here
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IUserRegistrationService, UserRegistrationService>();

        return services;
    }
}