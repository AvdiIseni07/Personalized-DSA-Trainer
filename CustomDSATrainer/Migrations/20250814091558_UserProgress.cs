using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomDSATrainer.Migrations
{
    /// <inheritdoc />
    public partial class UserProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerviousDayLog",
                table: "UserProgress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PerviousDayLog",
                table: "UserProgress",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
