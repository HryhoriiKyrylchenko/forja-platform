namespace Forja.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ForjaDbContext>("forjadb");

        // Register repositories or other infrastructure services here
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddHttpClient<IKeycloakClient, KeycloakClient>();
            
        return builder;
    }
}