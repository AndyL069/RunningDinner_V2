using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class AddressAdditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressAdditions",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    InvitationEmail = table.Column<string>(nullable: true),
                    SentTime = table.Column<DateTime>(nullable: false),
                    InvitationAccepted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_DinnerEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "DinnerEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_EventId",
                table: "Invitations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_UserId",
                table: "Invitations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropColumn(
                name: "AddressAdditions",
                table: "AspNetUsers");
        }
    }
}
