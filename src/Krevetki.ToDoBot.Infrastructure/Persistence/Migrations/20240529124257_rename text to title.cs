using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoBot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class renametexttotitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "ToDoItem",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ToDoItem",
                newName: "Text");
        }
    }
}
