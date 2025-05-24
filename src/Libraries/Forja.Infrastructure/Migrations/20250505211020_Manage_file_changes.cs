using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Manage_file_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddonFiles",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GameFiles",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GameVersions",
                schema: "games");

            migrationBuilder.DropColumn(
                name: "StorageUrl",
                schema: "games",
                table: "GameAddons");

            migrationBuilder.DropColumn(
                name: "Platforms",
                schema: "games",
                table: "Products");
            
            migrationBuilder.AddColumn<int[]>(
                name: "Platforms",
                schema: "games",
                table: "Products",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int>(
                name: "Platform",
                schema: "games",
                table: "GamePatches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductVersions",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Platform = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    StorageUrl = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    Changelog = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVersions_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "games",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFiles",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    IsArchive = table.Column<bool>(type: "boolean", nullable: false),
                    StorageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFiles_ProductVersions_ProductVersionId",
                        column: x => x.ProductVersionId,
                        principalSchema: "games",
                        principalTable: "ProductVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFiles_ProductVersionId",
                schema: "games",
                table: "ProductFiles",
                column: "ProductVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersions_ProductId",
                schema: "games",
                table: "ProductVersions",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductFiles",
                schema: "games");

            migrationBuilder.DropTable(
                name: "ProductVersions",
                schema: "games");

            migrationBuilder.DropColumn(
                name: "Platform",
                schema: "games",
                table: "GamePatches");

            migrationBuilder.DropColumn(
                name: "Platforms",
                schema: "games",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Platforms",
                schema: "games",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StorageUrl",
                schema: "games",
                table: "GameAddons",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AddonFiles",
                schema: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameAddonId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    IsArchive = table.Column<bool>(type: "boolean", nullable: false),
                    StorageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddonFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddonFiles_GameAddons_GameAddonId",
                        column: x => x.GameAddonId,
                        principalSchema: "games",
                        principalTable: "GameAddons",
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
                    Changelog = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StorageUrl = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
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
                    IsArchive = table.Column<bool>(type: "boolean", nullable: false),
                    StorageUrl = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AddonFiles_GameAddonId",
                schema: "games",
                table: "AddonFiles",
                column: "GameAddonId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFiles_GameVersionId",
                schema: "games",
                table: "GameFiles",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameVersions_GameId",
                schema: "games",
                table: "GameVersions",
                column: "GameId");
        }
    }
}
