using Forja.Infrastructure.Repositories.Common;
using Forja.Infrastructure.Repositories.Support;

namespace Forja.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ForjaDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

        // Register repositories or other infrastructure services here
        builder.Services.AddSingleton<IEmailService, EmailService>();
        builder.Services.AddHttpClient<IKeycloakClient, KeycloakClient>();
        
        builder.Services.Configure<MinioConfiguration>(builder.Configuration.GetSection("MinIO"));

        builder.Services.AddScoped<IStorageService, StorageService>(provider =>
        {
            var minioConfig = provider.GetRequiredService<IOptions<MinioConfiguration>>().Value;

            return new StorageService(
                minioConfig.Endpoint,
                minioConfig.AccessKey,
                minioConfig.SecretKey,
                minioConfig.DefaultBucketName,
                minioConfig.UseSSL
            );
        });

        builder.Services.AddScoped<ILegalDocumentRepository, LegalDocumentRepository>();
        builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
        
        builder.Services.AddScoped<ITestPaymentService, TestPaymentService>();
        
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
        
        builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
        
        builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
        builder.Services.AddScoped<IGameSaveRepository, GameSaveRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
        builder.Services.AddScoped<IUserFollowerRepository, UserFollowerRepository>();
        builder.Services.AddScoped<IUserLibraryAddonRepository, UserLibraryAddonRepository>();
        builder.Services.AddScoped<IUserLibraryGameRepository, UserLibraryGameRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserWishListRepository, UserWishListRepository>();

        builder.Services.AddScoped<IFAQRepository, FAQRepository>();
        builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
        builder.Services.AddScoped<ITicketMessageRepository, TicketMessageRepository>();
            
        return builder;
    }
}