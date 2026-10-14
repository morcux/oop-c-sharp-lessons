using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentsStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_themes_ThemeId",
                table: "payments");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "payments",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "payments",
                type: "text",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_themes_ThemeId",
                table: "payments",
                column: "ThemeId",
                principalTable: "themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_themes_ThemeId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "payments");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "payments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_themes_ThemeId",
                table: "payments",
                column: "ThemeId",
                principalTable: "themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
