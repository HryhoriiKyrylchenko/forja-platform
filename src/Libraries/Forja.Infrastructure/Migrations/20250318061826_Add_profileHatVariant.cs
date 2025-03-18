using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Forja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_profileHatVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<short>(

                name: "ProfileHatVariant",
                schema: "user-profile",
                table: "Users",
                type: "smallint",
                nullable: true);
        }
           
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "ProfileHatVariant",
               schema: "user-profile",
               table: "Users");
        }
    }
}
