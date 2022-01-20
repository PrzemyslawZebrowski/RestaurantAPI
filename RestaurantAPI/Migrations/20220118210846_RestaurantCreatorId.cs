using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace RestaurantAPI.Migrations
{
    public partial class RestaurantCreatorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CreatedByUserId",
                table: "Restaurants",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_CreatedByUserId",
                table: "Restaurants",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_CreatedByUserId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_CreatedByUserId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
