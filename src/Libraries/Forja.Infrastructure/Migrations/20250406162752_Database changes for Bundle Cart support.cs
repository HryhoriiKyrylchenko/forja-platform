using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabasechangesforBundleCartsupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Products_ProductId",
                schema: "store",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_ProductId",
                schema: "store",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "store",
                table: "Discounts");

            migrationBuilder.AddColumn<Guid>(
                name: "BundleId",
                schema: "store",
                table: "CartItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                schema: "games",
                table: "Bundles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DistributedPrice",
                schema: "games",
                table: "BundleProducts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_BundleId",
                schema: "store",
                table: "CartItems",
                column: "BundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Bundles_BundleId",
                schema: "store",
                table: "CartItems",
                column: "BundleId",
                principalSchema: "games",
                principalTable: "Bundles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Bundles_BundleId",
                schema: "store",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_BundleId",
                schema: "store",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "BundleId",
                schema: "store",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                schema: "games",
                table: "Bundles");

            migrationBuilder.DropColumn(
                name: "DistributedPrice",
                schema: "games",
                table: "BundleProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "store",
                table: "Discounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ProductId",
                schema: "store",
                table: "Discounts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Products_ProductId",
                schema: "store",
                table: "Discounts",
                column: "ProductId",
                principalSchema: "games",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
