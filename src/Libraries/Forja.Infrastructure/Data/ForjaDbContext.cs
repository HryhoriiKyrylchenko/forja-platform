using Forja.Domain.Entities.Common;

namespace Forja.Infrastructure.Data;

public class ForjaDbContext : DbContext
{
    public ForjaDbContext(DbContextOptions<ForjaDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    // Analytics entities (schema: analytics)
    public DbSet<AnalyticsAggregate> AnalyticsAggregates { get; set; }
    public DbSet<AnalyticsEvent> AnalyticsEvents { get; set; }
    public DbSet<AnalyticsSession> AnalyticsSessions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    
    // Analytics entities (schema: analytics)
    public DbSet<NewsArticle> NewsArticles { get; set; }
    public DbSet<LegalDocument> LegalDocuments { get; set; }
    
    // Products and related entities (schema: games)
    public DbSet<Bundle> Bundles { get; set; }
    public DbSet<BundleProduct> BundleProducts { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameAddon> GameAddons { get; set; }
    public DbSet<ProductGenres> ProductGenres { get; set; }
    public DbSet<GameTag> GameTags { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<MatureContent> MatureContents { get; set; }
    public DbSet<ProductMatureContent> ProductMatureContents { get; set; }
    public DbSet<ItemImage> ItemImages { get; set; }
    public DbSet<ProductImages> ProductImages { get; set; }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<GameMechanic> GameMechanics { get; set; }

    // Orders and Payments (schema: store)
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
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
    public DbSet<UserWishList> UserWishList { get; set; }
    public DbSet<UserFollower> UserFollowers { get; set; }
    public DbSet<GameSave> GameSaves { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.CustomUrl)
            .IsUnique();
        
        modelBuilder.Entity<UserFollower>()
            .HasOne(uf => uf.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<UserFollower>()
            .HasOne(uf => uf.Followed)
            .WithMany(u => u.Followers)
            .HasForeignKey(uf => uf.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}