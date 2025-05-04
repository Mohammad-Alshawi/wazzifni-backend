using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class addisspecialtorequestoncourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "CourseRegistrationRequests",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecial",
                table: "CourseRegistrationRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSpecial",
                table: "CourseRegistrationRequests");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "CourseRegistrationRequests",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }
    }
}
