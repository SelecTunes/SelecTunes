using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SelecTunesBackend.Migrations
{
    public partial class InitialDatabaseState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    IsBanned = table.Column<bool>(nullable: false),
                    IsHost = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    PartyId = table.Column<int>(nullable: true),
                    SpotifyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    PartyUID = table.Column<int>(nullable: false),
                    JoinCode = table.Column<string>(nullable: true),
                    PartyHostId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_User_PartyHostId",
                        column: x => x.PartyHostId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_PartyHostId",
                table: "Parties",
                column: "PartyHostId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PartyId",
                table: "User",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Parties_PartyId",
                table: "User",
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
                name: "FK_Parties_User_PartyHostId",
                table: "Parties");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Parties");
        }
    }
}
