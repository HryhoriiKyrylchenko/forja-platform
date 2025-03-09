namespace Forja.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ForjaDbContext>("forjadb");

        // Register repositories or other infrastructure services here
        builder.Services.AddSingleton<IEmailService, EmailService>();
        builder.Services.AddHttpClient<IKeycloakClient, KeycloakClient>();
        
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
        builder.Services.AddScoped<IGameSaveRepository, GameSaveRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
        builder.Services.AddScoped<IUserFollowerRepository, UserFollowerRepository>();
        builder.Services.AddScoped<IUserLibraryAddonRepository, UserLibraryAddonRepository>();
        builder.Services.AddScoped<IUserLibraryGameRepository, UserLibraryGameRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserWishListRepository, UserWishListRepository>();
            
        return builder;
    }
}