using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class Routes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressAdditions",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressNumber",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: true),
                    HostTeamId = table.Column<int>(nullable: true),
                    GuestTeam1Id = table.Column<int>(nullable: true),
                    GuestTeam2Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_DinnerEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "DinnerEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Teams_GuestTeam1Id",
                        column: x => x.GuestTeam1Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Teams_GuestTeam2Id",
                        column: x => x.GuestTeam2Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Teams_HostTeamId",
                        column: x => x.HostTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_EventId",
                table: "Routes",
                column: "EventId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AddressAdditions",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AddressNumber",
                table: "Teams");
        }
    }
}
