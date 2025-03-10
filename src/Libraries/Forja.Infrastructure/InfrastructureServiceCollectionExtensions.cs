namespace Forja.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ForjaDbContext>("forjadb");

        // Register repositories or other infrastructure services here
        builder.Services.AddSingleton<IEmailService, EmailService>();
        builder.Services.AddHttpClient<IKeycloakClient, KeycloakClient>();
        
        builder.Services.AddScoped<IBundleRepository, BundleRepository>();
        builder.Services.AddScoped<IBundleProductRepository, BundleProductRepository>();
        builder.Services.AddScoped<IGameAddonRepository, GameAddonRepository>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IGameMechanicRepository, GameMechanicRepository>();
        builder.Services.AddScoped<IGameTagRepository, GameTagRepository>();
        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
        builder.Services.AddScoped<IItemImageRepository, ItemImageRepository>();
        builder.Services.AddScoped<IMatureContentRepository, MatureContentRepository>();
        builder.Services.AddScoped<IMechanicRepository, MechanicRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductGenresRepository, ProductGenresRepository>();
        builder.Services.AddScoped<IProductImagesRepository, ProductImagesRepository>();
        builder.Services.AddScoped<IProductMatureContentRepository, ProductMatureContentRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        
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