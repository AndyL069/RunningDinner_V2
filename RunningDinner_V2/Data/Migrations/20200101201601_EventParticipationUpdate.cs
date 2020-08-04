using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class EventParticipationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipations",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "EventParticipationId",
                table: "EventParticipations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EventParticipations",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipations",
                table: "EventParticipations",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipations",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EventParticipations");

            migrationBuilder.AddColumn<int>(
                name: "EventParticipationId",
                table: "EventParticipations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipations",
                table: "EventParticipations",
                column: "EventParticipationId");
        }
    }
}
