using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomDSATrainer.Migrations
{
    /// <inheritdoc />
    public partial class ActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.AlterColumn<string>(
                name: "DaysLogged",
                table: "UserProgress",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<uint>(
                name: "TotalDaysLogged",
                table: "UserProgress",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDaysLogged",
                table: "UserProgress");

            migrationBuilder.AlterColumn<uint>(
                name: "DaysLogged",
                table: "UserProgress",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                });
        }
    }
}
