using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class contactId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "AspNetUsers");
        }
    }
}
