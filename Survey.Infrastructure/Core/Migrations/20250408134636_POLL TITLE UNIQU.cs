using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Core.Migrations
{
    /// <inheritdoc />
    public partial class POLLTITLEUNIQU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Polls_Id",
                table: "Polls");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_Title",
                table: "Polls",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Polls_Title",
                table: "Polls");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_Id",
                table: "Polls",
                column: "Id",
                unique: true);
        }
    }
}
