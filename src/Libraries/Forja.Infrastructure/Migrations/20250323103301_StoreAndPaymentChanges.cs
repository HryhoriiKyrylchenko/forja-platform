using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StoreAndPaymentChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                schema: "store",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                schema: "store",
                table: "Orders",
                newName: "Status");

            migrationBuilder.Sql(
                "UPDATE store.\"Payments\" SET \"ProviderResponse\" = 0 WHERE \"ProviderResponse\" IS NULL OR NOT \"ProviderResponse\" ~ '^[0-9]+$';");

            migrationBuilder.Sql(
                "ALTER TABLE store.\"Payments\" ALTER COLUMN \"ProviderResponse\" SET DATA TYPE integer USING \"ProviderResponse\"::integer;");

            migrationBuilder.Sql(
                "UPDATE store.\"Payments\" SET \"ProviderName\" = 0 WHERE \"ProviderName\" IS NULL OR NOT \"ProviderName\" ~ '^[0-9]+$';");

            migrationBuilder.Sql(
                "ALTER TABLE store.\"Payments\" ALTER COLUMN \"ProviderName\" SET DATA TYPE integer USING \"ProviderName\"::integer;");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalPaymentId",
                schema: "store",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "store",
                table: "Orders",
                newName: "PaymentStatus");

            migrationBuilder.Sql(
                "ALTER TABLE store.\"Payments\" ALTER COLUMN \"ProviderResponse\" SET DATA TYPE text USING \"ProviderResponse\"::text;");

            migrationBuilder.Sql(
                "ALTER TABLE store.\"Payments\" ALTER COLUMN \"ProviderName\" SET DATA TYPE text USING \"ProviderName\"::text;");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalPaymentId",
                schema: "store",
                table: "Payments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                schema: "store",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
