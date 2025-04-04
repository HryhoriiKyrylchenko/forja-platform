using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductModelsChangesForOptimizeStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageUrl",
                schema: "games",
                table: "Games");

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

            migrationBuilder.CreateIndex(
                name: "IX_GameFiles_GameVersionId",
                schema: "games",
                table: "GameFiles",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePatches_GameId",
                schema: "games",
                table: "GamePatches",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameVersions_GameId",
                schema: "games",
                table: "GameVersions",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameFiles",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GamePatches",
                schema: "games");

            migrationBuilder.DropTable(
                name: "GameVersions",
                schema: "games");

            migrationBuilder.AddColumn<string>(
                name: "StorageUrl",
                schema: "games",
                table: "Games",
                type: "text",
                nullable: true);
        }
    }
}
