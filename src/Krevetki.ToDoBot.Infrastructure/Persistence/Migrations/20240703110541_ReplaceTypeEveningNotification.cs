using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Krevetki.ToDoBot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceTypeEveningNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EveningNotificationTime",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "EveningNotificationStatus",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EveningNotificationStatus",
                table: "Users");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EveningNotificationTime",
                table: "Users",
                type: "time without time zone",
                nullable: true);
        }
    }
}
