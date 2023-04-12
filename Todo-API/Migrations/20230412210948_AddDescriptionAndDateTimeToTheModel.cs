using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndDateTimeToTheModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskTimestamp",
                table: "TodoItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "TaskTimestamp" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Description", "TaskTimestamp" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Description", "TaskTimestamp" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "TaskTimestamp",
                table: "TodoItems");
        }
    }
}
