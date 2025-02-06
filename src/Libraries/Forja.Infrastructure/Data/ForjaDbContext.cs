namespace Forja.Infrastructure.Data;

public class ForjaDbContext : DbContext
{
    public ForjaDbContext(DbContextOptions<ForjaDbContext> options) : base(options) { }
    
    // Analytics entities (schema: analytics)
    public DbSet<AnalyticsAggregate> AnalyticsAggregates { get; set; }
    public DbSet<AnalyticsEvent> AnalyticsEvents { get; set; }
    public DbSet<AnalyticsSession> AnalyticsSessions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    
    // Products and related entities (schema: games)
    public DbSet<Bundle> Bundles { get; set; }
    public DbSet<BundleProduct> BundleProducts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameAddon> GameAddons { get; set; }
    public DbSet<GameCategory> GameCategories { get; set; }
    public DbSet<GameTag> GameTags { get; set; }

    // Orders and Payments (schema: store)
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ProductDiscount> ProductDiscounts { get; set; }
    
    // Support entities (schema: support)
    public DbSet<FAQ> FAQs { get; set; }
    public DbSet<SupportTicket> SupportTickets { get; set; }
    public DbSet<TicketMessage> TicketMessages { get; set; }

    // User related entities (schema: user-profile)
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
    public DbSet<UserLibraryAddon> UserLibraryAddons { get; set; }
    public DbSet<UserLibraryGame> UserLibraryGames { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasDiscriminator<string>("ProductType")
            .HasValue<Game>("Game")
            .HasValue<GameAddon>("GameAddon")
            .HasValue<Bundle>("Bundle");

    }
}