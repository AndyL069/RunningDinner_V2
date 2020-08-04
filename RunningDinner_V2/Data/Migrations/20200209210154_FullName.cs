using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class FullName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHost_DinnerEvents_EventId",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHost_Teams_GuestTeam1Id",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHost_Teams_GuestTeam2Id",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHost_Teams_HostTeamId",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHost_FirstCourseId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHost_SecondCourseId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHost_ThirdCourseId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DinnerHost",
                table: "DinnerHosts");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHost_HostTeamId",
                table: "DinnerHosts",
                newName: "IX_DinnerHosts_HostTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHost_GuestTeam2Id",
                table: "DinnerHosts",
                newName: "IX_DinnerHosts_GuestTeam2Id");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHost_GuestTeam1Id",
                table: "DinnerHosts",
                newName: "IX_DinnerHosts_GuestTeam1Id");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHost_EventId",
                table: "DinnerHosts",
                newName: "IX_DinnerHosts_EventId");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DinnerHosts",
                table: "DinnerHosts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHosts_DinnerEvents_EventId",
                table: "DinnerHosts",
                column: "EventId",
                principalTable: "DinnerEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHosts_Teams_GuestTeam1Id",
                table: "DinnerHosts",
                column: "GuestTeam1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHosts_Teams_GuestTeam2Id",
                table: "DinnerHosts",
                column: "GuestTeam2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHosts_Teams_HostTeamId",
                table: "DinnerHosts",
                column: "HostTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_DinnerHosts_FirstCourseId",
                table: "Routes",
                column: "FirstCourseId",
                principalTable: "DinnerHosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_DinnerHosts_SecondCourseId",
                table: "Routes",
                column: "SecondCourseId",
                principalTable: "DinnerHosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_DinnerHosts_ThirdCourseId",
                table: "Routes",
                column: "ThirdCourseId",
                principalTable: "DinnerHosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHosts_DinnerEvents_EventId",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHosts_Teams_GuestTeam1Id",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHosts_Teams_GuestTeam2Id",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DinnerHosts_Teams_HostTeamId",
                table: "DinnerHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHosts_FirstCourseId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHosts_SecondCourseId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHosts_ThirdCourseId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DinnerHosts",
                table: "DinnerHosts");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "DinnerHosts",
                newName: "DinnerHost");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHosts_HostTeamId",
                table: "DinnerHost",
                newName: "IX_DinnerHost_HostTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHosts_GuestTeam2Id",
                table: "DinnerHost",
                newName: "IX_DinnerHost_GuestTeam2Id");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHosts_GuestTeam1Id",
                table: "DinnerHost",
                newName: "IX_DinnerHost_GuestTeam1Id");

            migrationBuilder.RenameIndex(
                name: "IX_DinnerHosts_EventId",
                table: "DinnerHost",
                newName: "IX_DinnerHost_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DinnerHost",
                table: "DinnerHost",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHost_DinnerEvents_EventId",
                table: "DinnerHost",
                column: "EventId",
                principalTable: "DinnerEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHost_Teams_GuestTeam1Id",
                table: "DinnerHost",
                column: "GuestTeam1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHost_Teams_GuestTeam2Id",
                table: "DinnerHost",
                column: "GuestTeam2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DinnerHost_Teams_HostTeamId",
                table: "DinnerHost",
                column: "HostTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_DinnerHost_FirstCourseId",
                table: "Routes",
                column: "FirstCourseId",
                principalTable: "DinnerHost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_DinnerHost_SecondCourseId",
                table: "Routes",
                column: "SecondCourseId",
                principalTable: "DinnerHost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_DinnerHost_ThirdCourseId",
                table: "Routes",
                column: "ThirdCourseId",
                principalTable: "DinnerHost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
