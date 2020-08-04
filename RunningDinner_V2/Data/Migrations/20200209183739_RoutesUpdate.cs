using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class RoutesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_GuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_GuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_HostTeamId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_GuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_GuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_HostTeamId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "GuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "GuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "HostTeamId",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "FirstCourseGuestTeam1Id",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirstCourseGuestTeam2Id",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirstCourseHostTeamId",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondCourseGuestTeam1Id",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondCourseGuestTeam2Id",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondCourseHostTeamId",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdCourseGuestTeam1Id",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdCourseGuestTeam2Id",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdCourseHostTeamId",
                table: "Routes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FirstCourseGuestTeam1Id",
                table: "Routes",
                column: "FirstCourseGuestTeam1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FirstCourseGuestTeam2Id",
                table: "Routes",
                column: "FirstCourseGuestTeam2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FirstCourseHostTeamId",
                table: "Routes",
                column: "FirstCourseHostTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SecondCourseGuestTeam1Id",
                table: "Routes",
                column: "SecondCourseGuestTeam1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SecondCourseGuestTeam2Id",
                table: "Routes",
                column: "SecondCourseGuestTeam2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SecondCourseHostTeamId",
                table: "Routes",
                column: "SecondCourseHostTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ThirdCourseGuestTeam1Id",
                table: "Routes",
                column: "ThirdCourseGuestTeam1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ThirdCourseGuestTeam2Id",
                table: "Routes",
                column: "ThirdCourseGuestTeam2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ThirdCourseHostTeamId",
                table: "Routes",
                column: "ThirdCourseHostTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_FirstCourseGuestTeam1Id",
                table: "Routes",
                column: "FirstCourseGuestTeam1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_FirstCourseGuestTeam2Id",
                table: "Routes",
                column: "FirstCourseGuestTeam2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_FirstCourseHostTeamId",
                table: "Routes",
                column: "FirstCourseHostTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_SecondCourseGuestTeam1Id",
                table: "Routes",
                column: "SecondCourseGuestTeam1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_SecondCourseGuestTeam2Id",
                table: "Routes",
                column: "SecondCourseGuestTeam2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_SecondCourseHostTeamId",
                table: "Routes",
                column: "SecondCourseHostTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_ThirdCourseGuestTeam1Id",
                table: "Routes",
                column: "ThirdCourseGuestTeam1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_ThirdCourseGuestTeam2Id",
                table: "Routes",
                column: "ThirdCourseGuestTeam2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_ThirdCourseHostTeamId",
                table: "Routes",
                column: "ThirdCourseHostTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_FirstCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_FirstCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_FirstCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_SecondCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_SecondCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_SecondCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_ThirdCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_ThirdCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_ThirdCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_FirstCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_FirstCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_FirstCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_SecondCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_SecondCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_SecondCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ThirdCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ThirdCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ThirdCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "FirstCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "FirstCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "FirstCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "SecondCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "SecondCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "SecondCourseHostTeamId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ThirdCourseGuestTeam1Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ThirdCourseGuestTeam2Id",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ThirdCourseHostTeamId",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "GuestTeam1Id",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestTeam2Id",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HostTeamId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_GuestTeam1Id",
                table: "Routes",
                column: "GuestTeam1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_GuestTeam2Id",
                table: "Routes",
                column: "GuestTeam2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_HostTeamId",
                table: "Routes",
                column: "HostTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_GuestTeam1Id",
                table: "Routes",
                column: "GuestTeam1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_GuestTeam2Id",
                table: "Routes",
                column: "GuestTeam2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_HostTeamId",
                table: "Routes",
                column: "HostTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
