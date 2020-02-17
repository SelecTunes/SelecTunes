using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SelecTunes.Backend.Migrations
{
    public partial class FixAuthReferenceNonExist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<int>(
                name: "PartyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PartyId",
                table: "AspNetUsers",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Parties_PartyId",
                table: "AspNetUsers",
                column: "PartyId",
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
                name: "FK_AspNetUsers_Parties_PartyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PartyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PartyId",
                table: "AspNetUsers");
        }
    }
}
