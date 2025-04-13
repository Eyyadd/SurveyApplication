using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Core.Migrations
{
    /// <inheritdoc />
    public partial class UPDATED : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Answers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Answers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Answers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Answers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_CreatedByUserId",
                table: "Answers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UpdatedByUserId",
                table: "Answers",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_CreatedByUserId",
                table: "Answers",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_UpdatedByUserId",
                table: "Answers",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_CreatedByUserId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_UpdatedByUserId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_CreatedByUserId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_UpdatedByUserId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Answers");
        }
    }
}
