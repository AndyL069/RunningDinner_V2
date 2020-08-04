using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class DinnerEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DinnerEvents",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventDate = table.Column<DateTime>(nullable: false),
                    LastRegisterDate = table.Column<DateTime>(nullable: false),
                    EventName = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    RoutesOpen = table.Column<bool>(nullable: false),
                    PartyLocation = table.Column<string>(nullable: true),
                    PartyLocationName = table.Column<string>(nullable: true),
                    EventPictureLink = table.Column<string>(nullable: true),
                    EventEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DinnerEvents", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DinnerEvents");
        }
    }
}
