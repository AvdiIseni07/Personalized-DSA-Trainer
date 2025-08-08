using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomDSATrainer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Problem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Statement = table.Column<string>(type: "TEXT", nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", nullable: false),
                    Categories = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PathToExecutable = table.Column<string>(type: "TEXT", nullable: false),
                    Result = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageExecutionTime = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestCase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PathToInputFile = table.Column<string>(type: "TEXT", nullable: false),
                    PathToOutputFile = table.Column<string>(type: "TEXT", nullable: false),
                    PathToExpectedOutputFile = table.Column<string>(type: "TEXT", nullable: false),
                    TimeLimit = table.Column<decimal>(type: "TEXT", nullable: false),
                    MemoryLimit = table.Column<uint>(type: "INTEGER", nullable: false),
                    TimeOfStarting = table.Column<long>(type: "INTEGER", nullable: false),
                    ExecutionTime = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TotalAttemptedTasks = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalSolvedProblems = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalUnsolvedProblems = table.Column<uint>(type: "INTEGER", nullable: false),
                    DaysLogged = table.Column<uint>(type: "INTEGER", nullable: false),
                    LoggingStreak = table.Column<uint>(type: "INTEGER", nullable: false),
                    PerviousDayLog = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgress", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Problem");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TestCase");

            migrationBuilder.DropTable(
                name: "UserProgress");
        }
    }
}
