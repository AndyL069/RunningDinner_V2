using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class EventParticipationUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InvitationMailSent",
                table: "EventParticipations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDinnerSaver",
                table: "EventParticipations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWithoutPartner",
                table: "EventParticipations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PartnerEmail",
                table: "EventParticipations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerLastName",
                table: "EventParticipations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerName",
                table: "EventParticipations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RegistrationDataIsComplete",
                table: "EventParticipations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SelectedCourse",
                table: "EventParticipations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedKitchenOption",
                table: "EventParticipations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationMailSent",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "IsDinnerSaver",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "IsWithoutPartner",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "PartnerEmail",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "PartnerLastName",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "PartnerName",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "RegistrationDataIsComplete",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "SelectedCourse",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "SelectedKitchenOption",
                table: "EventParticipations");
        }
    }
}
