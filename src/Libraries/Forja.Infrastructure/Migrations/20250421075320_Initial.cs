using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "user-profile");

            migrationBuilder.EnsureSchema(
                name: "analytics");

            migrationBuilder.EnsureSchema(
                name: "games");

            migrationBuilder.EnsureSchema(
                name: "store");

            migrationBuilder.EnsureSchema(
                name: "support");

            migrationBuilder.EnsureSchema(
                name: "common");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "AnalyticsAggregates",
                schema: "analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetricName = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsAggregates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bundles",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountType = table.Column<int>(type: "integer", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                schema: "support",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemImages",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ImageAlt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalDocuments",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileContent = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatureContents",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatureContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mechanics",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mechanics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Developer = table.Column<string>(type: "text", nullable: false),
                    MinimalAge = table.Column<int>(type: "integer", nullable: false),
                    Platforms = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    InterfaceLanguages = table.Column<string>(type: "text", nullable: false),
                    AudioLanguages = table.Column<string>(type: "text", nullable: false),
                    SubtitlesLanguages = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KeycloakUserId = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Firstname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Lastname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    City = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    SelfDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ShowPersonalInfo = table.Column<bool>(type: "boolean", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustomUrl = table.Column<string>(type: "text", nullable: true),
                    ProfileHatVariant = table.Column<short>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BundleProducts",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BundleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DistributedPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BundleProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BundleProducts_Bundles_BundleId",
                        column: x => x.BundleId,
                        principalSchema: "games",
                        principalTable: "Bundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BundleProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemRequirements = table.Column<string>(type: "text", nullable: true),
                    TimePlayed = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Products_Id",
                        column: x => x.Id,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDiscounts",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalSchema: "store",
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDiscounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductGenres",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    GenreId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "games",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductGenres_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemImageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_ItemImages_ItemImageId",
                        column: x => x.ItemImageId,
                        principalSchema: "games",
                        principalTable: "ItemImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMatureContents",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatureContentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMatureContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMatureContents_MatureContents_MatureContentId",
                        column: x => x.MatureContentId,
                        principalSchema: "games",
                        principalTable: "MatureContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMatureContents_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyticsEvents",
                schema: "analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsEvents_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnalyticsSessions",
                schema: "analytics",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Metadata = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_AnalyticsSessions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Details = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsArticles",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsPrioritized = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileContent = table.Column<byte[]>(type: "bytea", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsArticles_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NewsArticles_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PositiveRating = table.Column<bool>(type: "boolean", nullable: false),
                    Comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                schema: "support",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTickets_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFollowers",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollowers_Users_FollowedId",
                        column: x => x.FollowedId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFollowers_Users_FollowerId",
                        column: x => x.FollowerId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWishLists",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWishLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWishLists_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWishLists_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievements_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameAddons",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameAddons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameAddons_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameAddons_Products_Id",
                        column: x => x.Id,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameMechanics",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    MechanicId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMechanics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameMechanics_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameMechanics_Mechanics_MechanicId",
                        column: x => x.MechanicId,
                        principalSchema: "games",
                        principalTable: "Mechanics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePatches",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FromVersion = table.Column<string>(type: "text", nullable: false),
                    ToVersion = table.Column<string>(type: "text", nullable: false),
                    PatchUrl = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePatches_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTags",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameTags_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "games",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameVersions",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    StorageUrl = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    Changelog = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameVersions_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLibraryGames",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimePlayed = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLibraryGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLibraryGames_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "games",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLibraryGames_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    BundleId = table.Column<Guid>(type: "uuid", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Bundles_BundleId",
                        column: x => x.BundleId,
                        principalSchema: "games",
                        principalTable: "Bundles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalSchema: "store",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Carts_CartId",
                        column: x => x.CartId,
                        principalSchema: "store",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketMessages",
                schema: "support",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SupportTicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSupportAgent = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMessages_SupportTickets_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalSchema: "support",
                        principalTable: "SupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAchievements",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AchievementId = table.Column<Guid>(type: "uuid", nullable: false),
                    AchievedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "user-profile",
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameFiles",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    IsArchive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameFiles_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalSchema: "games",
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameSaves",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLibraryGameId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaveFileUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSaves_UserLibraryGames_UserLibraryGameId",
                        column: x => x.UserLibraryGameId,
                        principalSchema: "user-profile",
                        principalTable: "UserLibraryGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameSaves_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLibraryAddons",
                schema: "user-profile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLibraryGameId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddonId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLibraryAddons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLibraryAddons_GameAddons_AddonId",
                        column: x => x.AddonId,
                        principalSchema: "games",
                        principalTable: "GameAddons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLibraryAddons_UserLibraryGames_UserLibraryGameId",
                        column: x => x.UserLibraryGameId,
                        principalSchema: "user-profile",
                        principalTable: "UserLibraryGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExternalPaymentId = table.Column<string>(type: "text", nullable: false),
                    ProviderName = table.Column<int>(type: "integer", nullable: false),
                    ProviderResponse = table.Column<int>(type: "integer", nullable: false),
                    IsRefunded = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "store",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "user-profile",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_GameId",
                schema: "user-profile",
                table: "Achievements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_UserId",
                schema: "analytics",
                table: "AnalyticsEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsSessions_UserId",
                schema: "analytics",
                table: "AnalyticsSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                schema: "analytics",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BundleProducts_BundleId",
                schema: "games",
                table: "BundleProducts",
                column: "BundleId");

            migrationBuilder.CreateIndex(
                name: "IX_BundleProducts_ProductId",
                schema: "games",
                table: "BundleProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_BundleId",
                schema: "store",
                table: "CartItems",
                column: "BundleId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                schema: "store",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                schema: "store",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                schema: "store",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameAddons_GameId",
                schema: "games",
                table: "GameAddons",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFiles_GameVersionId",
                schema: "games",
                table: "GameFiles",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMechanics_GameId",
                schema: "games",
                table: "GameMechanics",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMechanics_MechanicId",
                schema: "games",
                table: "GameMechanics",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePatches_GameId",
                schema: "games",
                table: "GamePatches",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSaves_UserId",
                schema: "user-profile",
                table: "GameSaves",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSaves_UserLibraryGameId",
                schema: "user-profile",
                table: "GameSaves",
                column: "UserLibraryGameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_GameId",
                schema: "games",
                table: "GameTags",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_TagId",
                schema: "games",
                table: "GameTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_GameVersions_GameId",
                schema: "games",
                table: "GameVersions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_AuthorId",
                schema: "common",
                table: "NewsArticles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_ProductId",
                schema: "common",
                table: "NewsArticles",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartId",
                schema: "store",
                table: "Orders",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "store",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                schema: "store",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                schema: "store",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscounts_DiscountId",
                schema: "store",
                table: "ProductDiscounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscounts_ProductId",
                schema: "store",
                table: "ProductDiscounts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGenres_GenreId",
                schema: "games",
                table: "ProductGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGenres_ProductId",
                schema: "games",
                table: "ProductGenres",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ItemImageId",
                schema: "games",
                table: "ProductImages",
                column: "ItemImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                schema: "games",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMatureContents_MatureContentId",
                schema: "games",
                table: "ProductMatureContents",
                column: "MatureContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMatureContents_ProductId",
                schema: "games",
                table: "ProductMatureContents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                schema: "user-profile",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                schema: "user-profile",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_UserId",
                schema: "support",
                table: "SupportTickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_SenderId",
                schema: "support",
                table: "TicketMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_SupportTicketId",
                schema: "support",
                table: "TicketMessages",
                column: "SupportTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_AchievementId",
                schema: "user-profile",
                table: "UserAchievements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_UserId",
                schema: "user-profile",
                table: "UserAchievements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowers_FollowedId",
                schema: "user-profile",
                table: "UserFollowers",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowers_FollowerId",
                schema: "user-profile",
                table: "UserFollowers",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibraryAddons_AddonId",
                schema: "user-profile",
                table: "UserLibraryAddons",
                column: "AddonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibraryAddons_UserLibraryGameId",
                schema: "user-profile",
                table: "UserLibraryAddons",
                column: "UserLibraryGameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibraryGames_GameId",
                schema: "user-profile",
                table: "UserLibraryGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibraryGames_UserId",
                schema: "user-profile",
                table: "UserLibraryGames",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomUrl",
                schema: "user-profile",
                table: "Users",
                column: "CustomUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "user-profile",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "user-profile",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWishLists_ProductId",
                schema: "user-profile",
                table: "UserWishLists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWishLists_UserId",
                schema: "user-profile",
                table: "UserWishLists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalyticsAggregates",
                schema: "analytics");

            migrationBuilder.DropTable(
                name: "AnalyticsEvents",
                schema: "analytics");

            migrationBuilder.DropTable(
                name: "AnalyticsSessions",
                schema: "analytics");

            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "analytics");

            migrationBuilder.DropTable(
                name: "BundleProducts",
                schema: "games");

            migrationBuilder.DropTable(
                name: "CartItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "FAQs",
                schema: "support");

            migrationBuilder.DropTable(
                name: "GameFiles",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GameMechanics",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GamePatches",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GameSaves",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "GameTags",
                schema: "games");

            migrationBuilder.DropTable(
                name: "LegalDocuments",
                schema: "common");

            migrationBuilder.DropTable(
                name: "NewsArticles",
                schema: "common");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "store");

            migrationBuilder.DropTable(
                name: "ProductDiscounts",
                schema: "store");

            migrationBuilder.DropTable(
                name: "ProductGenres",
                schema: "games");

            migrationBuilder.DropTable(
                name: "ProductImages",
                schema: "games");

            migrationBuilder.DropTable(
                name: "ProductMatureContents",
                schema: "games");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "TicketMessages",
                schema: "support");

            migrationBuilder.DropTable(
                name: "UserAchievements",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "UserFollowers",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "UserLibraryAddons",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "UserWishLists",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "Bundles",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GameVersions",
                schema: "games");

            migrationBuilder.DropTable(
                name: "Mechanics",
                schema: "games");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "games");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Discounts",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "games");

            migrationBuilder.DropTable(
                name: "ItemImages",
                schema: "games");

            migrationBuilder.DropTable(
                name: "MatureContents",
                schema: "games");

            migrationBuilder.DropTable(
                name: "SupportTickets",
                schema: "support");

            migrationBuilder.DropTable(
                name: "Achievements",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "GameAddons",
                schema: "games");

            migrationBuilder.DropTable(
                name: "UserLibraryGames",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "games");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "user-profile");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "games");
        }
    }
}
