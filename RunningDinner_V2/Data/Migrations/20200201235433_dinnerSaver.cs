using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class dinnerSaver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDinnerSaver",
                table: "EventParticipations");

            migrationBuilder.AddColumn<bool>(
                name: "DinnerSaver",
                table: "EventParticipations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DinnerSaver",
                table: "EventParticipations");

            migrationBuilder.AddColumn<bool>(
                name: "IsDinnerSaver",
                table: "EventParticipations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
