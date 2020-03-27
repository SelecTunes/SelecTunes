using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SelecTunes.Backend.Migrations
{
    public partial class KickAndBanUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<int>(
                name: "PartyId1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Strikes",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PartyId1",
                table: "AspNetUsers",
                column: "PartyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Parties_PartyId1",
                table: "AspNetUsers",
                column: "PartyId1",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Parties_PartyId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PartyId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PartyId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Strikes",
                table: "AspNetUsers");
        }
    }
}
