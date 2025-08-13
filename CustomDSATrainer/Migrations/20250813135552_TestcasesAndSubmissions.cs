using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomDSATrainer.Migrations
{
    /// <inheritdoc />
    public partial class TestcasesAndSubmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCase_Problem_ProblemId",
                table: "TestCase");

            migrationBuilder.RenameColumn(
                name: "ProblemId",
                table: "TestCase",
                newName: "SubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_TestCase_ProblemId",
                table: "TestCase",
                newName: "IX_TestCase_SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCase_Submissions_SubmissionId",
                table: "TestCase",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCase_Submissions_SubmissionId",
                table: "TestCase");

            migrationBuilder.RenameColumn(
                name: "SubmissionId",
                table: "TestCase",
                newName: "ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_TestCase_SubmissionId",
                table: "TestCase",
                newName: "IX_TestCase_ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCase_Problem_ProblemId",
                table: "TestCase",
                column: "ProblemId",
                principalTable: "Problem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
