using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class TeamUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: true),
                    Partner1Id = table.Column<string>(nullable: true),
                    Partner2Id = table.Column<string>(nullable: true),
                    DinnerSaver = table.Column<bool>(nullable: false),
                    SelectedCourse = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Allergies = table.Column<string>(nullable: true),
                    SelectedKitchenOption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_DinnerEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "DinnerEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_AspNetUsers_Partner1Id",
                        column: x => x.Partner1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_AspNetUsers_Partner2Id",
                        column: x => x.Partner2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_EventId",
                table: "Teams",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Partner1Id",
                table: "Teams",
                column: "Partner1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Partner2Id",
                table: "Teams",
                column: "Partner2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
