using Forja.Application.Interfaces.Common;
using Forja.Application.Services.Analytics;
using Forja.Application.Services.Common;

namespace Forja.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register your application services here
        services.AddScoped<IAnalyticsEventService, AnalyticsEventService>();
        services.AddScoped<IAnalyticsSessionService, AnalyticsSessionService>();
        services.AddScoped<IAnalyticsAggregateService, AnalyticsAggregateService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddTransient<IClaimsTransformation, KeycloakRolesClaimsTransformer>();

        services.AddScoped<ILegalDocumentService, LegalDocumentService>();
        services.AddScoped<INewsArticleService, NewsArticleService>();

        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IFilterDataService, FilterDataService>();

        services.AddScoped<IBundleProductService, BundleProductService>();
        services.AddScoped<IBundleService, BundleService>();
        services.AddScoped<IGameAddonService, GameAddonService>();
        services.AddScoped<IGameFileService, GameFileService>();
        services.AddScoped<IGameMechanicService, GameMechanicService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGameTagService, GameTagService>();
        services.AddScoped<IGameVersionService, GameVersionService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IItemImageService, ItemImageService>();
        services.AddScoped<IMatureContentService, MatureContentService>();
        services.AddScoped<IMechanicService, MechanicService>();
        services.AddScoped<IProductGenresService, ProductGenresService>();
        services.AddScoped<IProductImagesService, ProductImagesService>();
        services.AddScoped<IProductMatureContentService, ProductMatureContentService>();
        services.AddScoped<ITagService, TagService>();
        
        services.AddScoped<IFileManagerService, FileManagerService>();

        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPriceCalculator, PriceCalculator>();
        
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IGameSaveService, GameSaveService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserFollowerService, UserFollowerService>();
        services.AddScoped<IUserLibraryService, UserLibraryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserWishListService, UserWishListService>();

        services.AddScoped<IFAQService, FAQService>();
        services.AddScoped<ISupportTicketService, SupportTicketService>();
        services.AddScoped<ITicketMessageService, TicketMessageService>();
        
        return services;
    }
}