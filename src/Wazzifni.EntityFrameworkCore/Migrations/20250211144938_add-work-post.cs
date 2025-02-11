using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class addworkpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkPosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkEngagement = table.Column<byte>(type: "tinyint", nullable: false),
                    WorkLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    EducationLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    MinSalary = table.Column<decimal>(type: "decimal(20,5)", precision: 20, scale: 5, nullable: false),
                    MaxSalary = table.Column<decimal>(type: "decimal(20,5)", precision: 20, scale: 5, nullable: false),
                    ExperienceYearsCount = table.Column<int>(type: "int", nullable: false),
                    RequiredEmployeesCount = table.Column<int>(type: "int", nullable: false),
                    ApplicantsCount = table.Column<int>(type: "int", nullable: false),
                    WorkVisibility = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_WorkPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPosts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPosts_CompanyId",
                table: "WorkPosts",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkPosts");
        }
    }
}
