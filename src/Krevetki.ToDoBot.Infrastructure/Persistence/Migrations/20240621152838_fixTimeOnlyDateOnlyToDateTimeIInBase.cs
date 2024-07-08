using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Krevetki.ToDoBot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixTimeOnlyDateOnlyToDateTimeIInBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateToStart",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "TimeToStart",
                table: "ToDoItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeToStart",
                table: "ToDoItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeToStart",
                table: "ToDoItems");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateToStart",
                table: "ToDoItems",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeToStart",
                table: "ToDoItems",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
