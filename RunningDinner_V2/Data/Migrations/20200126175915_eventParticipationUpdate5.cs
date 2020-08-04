using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class eventParticipationUpdate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDataIsComplete",
                table: "EventParticipations");

            migrationBuilder.AddColumn<bool>(
                name: "RegistrationComplete",
                table: "EventParticipations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationComplete",
                table: "EventParticipations");

            migrationBuilder.AddColumn<bool>(
                name: "RegistrationDataIsComplete",
                table: "EventParticipations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
