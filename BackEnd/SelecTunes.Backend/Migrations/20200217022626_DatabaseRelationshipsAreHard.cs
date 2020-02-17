using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SelecTunes.Backend.Migrations
{
    public partial class DatabaseRelationshipsAreHard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropIndex(
                name: "IX_Parties_PartyHostId",
                table: "Parties");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_PartyHostId",
                table: "Parties",
                column: "PartyHostId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropIndex(
                name: "IX_Parties_PartyHostId",
                table: "Parties");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_PartyHostId",
                table: "Parties",
                column: "PartyHostId");
        }
    }
}
