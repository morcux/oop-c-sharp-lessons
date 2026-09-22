using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_videos_ThemeId",
                table: "videos");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "videos",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_videos_ThemeId_SequenceNumber",
                table: "videos",
                columns: new[] { "ThemeId", "SequenceNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_videos_ThemeId_SequenceNumber",
                table: "videos");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "videos");

            migrationBuilder.CreateIndex(
                name: "IX_videos_ThemeId",
                table: "videos",
                column: "ThemeId");
        }
    }
}
