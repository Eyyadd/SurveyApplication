using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Core.Migrations
{
    /// <inheritdoc />
    public partial class updatdPolls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Polls",
                newName: "Summary");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Polls",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndsAt",
                table: "Polls",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Polls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartsAt",
                table: "Polls",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Polls_Id",
                table: "Polls",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Polls_Id",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "EndsAt",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "StartsAt",
                table: "Polls");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Polls",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Polls",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
