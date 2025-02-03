using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class addspokenlanguagestoprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpokenLanguageValue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    SpokenLanguageId = table.Column<int>(type: "int", nullable: false),
                    OralLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    WritingLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    IsNative = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_SpokenLanguageValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpokenLanguageValue_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpokenLanguageValue_SpokenLanguages_SpokenLanguageId",
                        column: x => x.SpokenLanguageId,
                        principalTable: "SpokenLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpokenLanguageValue_ProfileId",
                table: "SpokenLanguageValue",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SpokenLanguageValue_SpokenLanguageId",
                table: "SpokenLanguageValue",
                column: "SpokenLanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpokenLanguageValue");
        }
    }
}
