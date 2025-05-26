using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazzifni.Migrations
{
    /// <inheritdoc />
    public partial class removetraineerelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_Trainees_TraineeId",
                table: "CourseComments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRates_Trainees_TraineeId",
                table: "CourseRates");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRegistrationRequests_Trainees_TraineeId",
                table: "CourseRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees");

            migrationBuilder.DropIndex(
                name: "IX_CourseRates_TraineeId",
                table: "CourseRates");

            migrationBuilder.DropColumn(
                name: "TraineeId",
                table: "CourseRates");

            migrationBuilder.DropColumn(
                name: "TraineeId",
                table: "AbpUsers");

            migrationBuilder.RenameColumn(
                name: "RegisteredTraineesCount",
                table: "Courses",
                newName: "RegisteredCount");

            migrationBuilder.RenameColumn(
                name: "TraineeId",
                table: "CourseRegistrationRequests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseRegistrationRequests_TraineeId",
                table: "CourseRegistrationRequests",
                newName: "IX_CourseRegistrationRequests_UserId");

            migrationBuilder.RenameColumn(
                name: "TraineeId",
                table: "CourseComments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseComments_TraineeId",
                table: "CourseComments",
                newName: "IX_CourseComments_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_AbpUsers_UserId",
                table: "CourseComments",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRegistrationRequests_AbpUsers_UserId",
                table: "CourseRegistrationRequests",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_AbpUsers_UserId",
                table: "CourseComments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRegistrationRequests_AbpUsers_UserId",
                table: "CourseRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees");

            migrationBuilder.RenameColumn(
                name: "RegisteredCount",
                table: "Courses",
                newName: "RegisteredTraineesCount");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CourseRegistrationRequests",
                newName: "TraineeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseRegistrationRequests_UserId",
                table: "CourseRegistrationRequests",
                newName: "IX_CourseRegistrationRequests_TraineeId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CourseComments",
                newName: "TraineeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseComments_UserId",
                table: "CourseComments",
                newName: "IX_CourseComments_TraineeId");

            migrationBuilder.AddColumn<long>(
                name: "TraineeId",
                table: "CourseRates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TraineeId",
                table: "AbpUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseRates_TraineeId",
                table: "CourseRates",
                column: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_Trainees_TraineeId",
                table: "CourseComments",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRates_Trainees_TraineeId",
                table: "CourseRates",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRegistrationRequests_Trainees_TraineeId",
                table: "CourseRegistrationRequests",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
