using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomDSATrainer.Migrations
{
    /// <inheritdoc />
    public partial class SeedKaggleDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfStarting",
                table: "TestCase");

            migrationBuilder.RenameColumn(
                name: "PathToOutputFile",
                table: "TestCase",
                newName: "PathToExecutable");

            migrationBuilder.AlterColumn<long>(
                name: "TimeLimit",
                table: "TestCase",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 9,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 20,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 39,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 189,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1138,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1821,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1909,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1928,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 2444,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 2829,
                column: "Categories",
                value: "Combinatorics");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 3359,
                column: "Categories",
                value: "Combinatorics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PathToExecutable",
                table: "TestCase",
                newName: "PathToOutputFile");

            migrationBuilder.AlterColumn<decimal>(
                name: "TimeLimit",
                table: "TestCase",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                name: "TimeOfStarting",
                table: "TestCase",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 9,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 20,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 39,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 189,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1138,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1821,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1909,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 1928,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 2444,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 2829,
                column: "Categories",
                value: "/");

            migrationBuilder.UpdateData(
                table: "Problem",
                keyColumn: "Id",
                keyValue: 3359,
                column: "Categories",
                value: "/");
        }
    }
}
