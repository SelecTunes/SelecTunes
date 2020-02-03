using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SelecTunes.Backend.Migrations
{
    public partial class UserModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_User_PartyHostId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Parties_PartyId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "PartyUID",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "BannedUsers");

            migrationBuilder.RenameIndex(
                name: "IX_User_PartyId",
                table: "BannedUsers",
                newName: "IX_BannedUsers_PartyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BannedUsers",
                table: "BannedUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BannedUsers_Parties_PartyId",
                table: "BannedUsers",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_BannedUsers_PartyHostId",
                table: "Parties",
                column: "PartyHostId",
                principalTable: "BannedUsers",
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
                name: "FK_BannedUsers_Parties_PartyId",
                table: "BannedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_BannedUsers_PartyHostId",
                table: "Parties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BannedUsers",
                table: "BannedUsers");

            migrationBuilder.RenameTable(
                name: "BannedUsers",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_BannedUsers_PartyId",
                table: "User",
                newName: "IX_User_PartyId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Parties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartyUID",
                table: "Parties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_User_PartyHostId",
                table: "Parties",
                column: "PartyHostId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Parties_PartyId",
                table: "User",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
