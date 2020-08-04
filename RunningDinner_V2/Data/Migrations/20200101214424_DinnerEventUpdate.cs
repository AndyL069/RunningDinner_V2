using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class DinnerEventUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipations_DinnerEvents_EventId",
                table: "EventParticipations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DinnerEvents",
                table: "DinnerEvents");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "DinnerEvents");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DinnerEvents",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DinnerEvents",
                table: "DinnerEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipations_DinnerEvents_EventId",
                table: "EventParticipations",
                column: "EventId",
                principalTable: "DinnerEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipations_DinnerEvents_EventId",
                table: "EventParticipations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DinnerEvents",
                table: "DinnerEvents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DinnerEvents");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "DinnerEvents",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DinnerEvents",
                table: "DinnerEvents",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipations_DinnerEvents_EventId",
                table: "EventParticipations",
                column: "EventId",
                principalTable: "DinnerEvents",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
