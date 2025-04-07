using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Core.Migrations
{
    /// <inheritdoc />
    public partial class addaudittable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Polls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Polls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_CreatedByUserId",
                table: "Polls",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_UpdatedByUserId",
                table: "Polls",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_CreatedByUserId",
                table: "Polls",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_UpdatedByUserId",
                table: "Polls",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_CreatedByUserId",
                table: "Polls");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_UpdatedByUserId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_CreatedByUserId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_UpdatedByUserId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Polls");
        }
    }
}
