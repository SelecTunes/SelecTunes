using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SelecTunes.Backend.Migrations
{
    public partial class ConsolidateUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_BannedUsers_PartyHostId",
                table: "Parties");

            migrationBuilder.DropTable(
                name: "BannedUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PartyHostId",
                table: "Parties",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyAccessToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyRefreshToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_AspNetUsers_PartyHostId",
                table: "Parties",
                column: "PartyHostId",
                principalTable: "AspNetUsers",
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
                name: "FK_Parties_AspNetUsers_PartyHostId",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpotifyAccessToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpotifyRefreshToken",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "PartyHostId",
                table: "Parties",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BannedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    PartyId = table.Column<int>(type: "integer", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    SpotifyAccessToken = table.Column<string>(type: "text", nullable: true),
                    SpotifyRefreshToken = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannedUsers_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannedUsers_PartyId",
                table: "BannedUsers",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_BannedUsers_PartyHostId",
                table: "Parties",
                column: "PartyHostId",
                principalTable: "BannedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
