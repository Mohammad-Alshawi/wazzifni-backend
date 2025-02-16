using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class addisclosedforwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkVisibility",
                table: "WorkPosts",
                newName: "WorkAvailbility");

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "WorkPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "WorkPosts");

            migrationBuilder.RenameColumn(
                name: "WorkAvailbility",
                table: "WorkPosts",
                newName: "WorkVisibility");
        }
    }
}
