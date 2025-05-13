using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dfsdfAdssssssssdUpdatedModelsdfsdfsdfs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId1",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_BookId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BookId1",
                table: "OrderItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "OrderItems",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BookTitle",
                table: "OrderItems",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BookTitle",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddColumn<Guid>(
                name: "BookId1",
                table: "OrderItems",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BookId1",
                table: "OrderItems",
                column: "BookId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId1",
                table: "OrderItems",
                column: "BookId1",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
