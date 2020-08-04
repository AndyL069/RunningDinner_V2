using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class participation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventParticipations",
                columns: table => new
                {
                    EventParticipationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    EventId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipations", x => x.EventParticipationId);
                    table.ForeignKey(
                        name: "FK_EventParticipations_DinnerEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "DinnerEvents",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventParticipations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipations_EventId",
                table: "EventParticipations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipations_UserId",
                table: "EventParticipations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventParticipations");
        }
    }
}
