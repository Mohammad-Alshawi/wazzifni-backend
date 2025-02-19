using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class addslugtoworjpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "WorkPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkPosts_Slug",
                table: "WorkPosts",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkPosts_Slug",
                table: "WorkPosts");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "WorkPosts");
        }
    }
}
