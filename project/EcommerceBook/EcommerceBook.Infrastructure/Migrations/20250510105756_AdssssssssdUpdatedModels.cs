using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdssssssssdUpdatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "Orders",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfilledDate",
                table: "Orders",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FulfilledDate",
                table: "Orders");
        }
    }
}
