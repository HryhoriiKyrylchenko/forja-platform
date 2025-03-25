using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewsArticlePropertyCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPrioritized",
                schema: "common",
                table: "NewsArticles",
                newName: "IsPrioritized");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPrioritized",
                schema: "common",
                table: "NewsArticles",
                newName: "IdPrioritized");
        }
    }
}
