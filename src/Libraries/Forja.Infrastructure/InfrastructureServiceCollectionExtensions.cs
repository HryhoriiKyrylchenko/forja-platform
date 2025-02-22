using Forja.Domain.Repositories.Games;
using Forja.Infrastructure.Repositories.Games;
using Forja.Infrastructure.Repositories.UserProfile;

namespace Forja.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ForjaDbContext>("forjadb");

        // Register repositories or other infrastructure services here
        builder.Services.AddScoped<IUserProfileUnitOfWork, UserProfileUnitOfWork>();
        builder.Services.AddHttpClient<IKeycloakClient, KeycloakClient>();
            
        return builder;
    }
}