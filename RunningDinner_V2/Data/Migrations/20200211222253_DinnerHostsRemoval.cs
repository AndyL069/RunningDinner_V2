using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class DinnerHostsRemoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHosts_FirstCourseId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHosts_SecondCourseId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_DinnerHosts_ThirdCourseId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "DinnerHosts");

            migrationBuilder.DropIndex(
                name: "IX_Routes_FirstCourseId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_SecondCourseId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ThirdCourseId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "FirstCourseId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "SecondCourseId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ThirdCourseId",
                table: "Routes");

            migrationBuilder.AddColumn<string>(
                name: "FullAddress",
                table: "Teams",
                nullable: true);

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
                name: "FullAddress",
                table: "Teams");

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
                name: "FirstCourseId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondCourseId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdCourseId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DinnerHosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Course = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestTeam1Id = table.Column<int>(type: "int", nullable: true),
                    GuestTeam2Id = table.Column<int>(type: "int", nullable: true),
                    HostTeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DinnerHosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DinnerHosts_DinnerEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "DinnerEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DinnerHosts_Teams_GuestTeam1Id",
                        column: x => x.GuestTeam1Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DinnerHosts_Teams_GuestTeam2Id",
                        column: x => x.GuestTeam2Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DinnerHosts_Teams_HostTeamId",
                        column: x => x.HostTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FirstCourseId",
                table: "Routes",
                column: "FirstCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SecondCourseId",
                table: "Routes",
                column: "SecondCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ThirdCourseId",
                table: "Routes",
                column: "ThirdCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_DinnerHosts_EventId",
                table: "DinnerHosts",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_DinnerHosts_GuestTeam1Id",
                table: "DinnerHosts",
                column: "GuestTeam1Id");

            migrationBuilder.CreateIndex(
                name: "IX_DinnerHosts_GuestTeam2Id",
                table: "DinnerHosts",
                column: "GuestTeam2Id");

            migrationBuilder.CreateIndex(
                name: "IX_DinnerHosts_HostTeamId",
                table: "DinnerHosts",
                column: "HostTeamId");

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
    }
}
