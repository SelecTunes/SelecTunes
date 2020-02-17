using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SelecTunes.Backend.Migrations
{
    public partial class AddTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "SpotifyAccessToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpotifyRefreshToken",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Token_AccessToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Token_CreateDate",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Token_ExpiresIn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token_RefreshToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token_Scope",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token_TokenType",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "Token_AccessToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Token_CreateDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Token_ExpiresIn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Token_RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Token_Scope",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Token_TokenType",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "SpotifyAccessToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyRefreshToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
