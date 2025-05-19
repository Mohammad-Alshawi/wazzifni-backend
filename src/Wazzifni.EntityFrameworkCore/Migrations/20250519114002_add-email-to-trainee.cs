using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class addemailtotrainee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Trainees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Trainees");
        }
    }
}
