using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class RouteForTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteForTeamId",
                table: "Routes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_RouteForTeamId",
                table: "Routes",
                column: "RouteForTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Teams_RouteForTeamId",
                table: "Routes",
                column: "RouteForTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Teams_RouteForTeamId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_RouteForTeamId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RouteForTeamId",
                table: "Routes");
        }
    }
}
