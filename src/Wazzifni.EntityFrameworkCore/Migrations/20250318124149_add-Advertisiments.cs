using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class addAdvertisiments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisiments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPostId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisiments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisiments_WorkPosts_WorkPostId",
                        column: x => x.WorkPostId,
                        principalTable: "WorkPosts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisiments_WorkPostId",
                table: "Advertisiments",
                column: "WorkPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisiments");
        }
    }
}
