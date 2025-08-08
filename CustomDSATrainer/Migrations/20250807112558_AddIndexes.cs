using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomDSATrainer.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProblemId",
                table: "TestCase",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Verdict",
                table: "TestCase",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProblemId",
                table: "Submissions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AIReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PathToCPPFile = table.Column<string>(type: "TEXT", nullable: false),
                    Review = table.Column<string>(type: "TEXT", nullable: false),
                    ProblemStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ProblemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIReview_Problem_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCase_ProblemId",
                table: "TestCase",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ProblemId",
                table: "Submissions",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "idx_Problem_Status",
                table: "Problem",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AIReview_ProblemId",
                table: "AIReview",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Problem_ProblemId",
                table: "Submissions",
                column: "ProblemId",
                principalTable: "Problem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCase_Problem_ProblemId",
                table: "TestCase",
                column: "ProblemId",
                principalTable: "Problem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Problem_ProblemId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestCase_Problem_ProblemId",
                table: "TestCase");

            migrationBuilder.DropTable(
                name: "AIReview");

            migrationBuilder.DropIndex(
                name: "IX_TestCase_ProblemId",
                table: "TestCase");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ProblemId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "idx_Problem_Status",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "TestCase");

            migrationBuilder.DropColumn(
                name: "Verdict",
                table: "TestCase");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "Submissions");
        }
    }
}
