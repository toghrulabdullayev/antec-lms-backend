using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntecLMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                schema: "public",
                table: "lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                schema: "public",
                table: "lessons");
        }
    }
}
