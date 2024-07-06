using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Krevetki.ToDoBot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixConvertEveningNotificationInString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EveningNotificationStatus",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EveningNotificationStatus",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
