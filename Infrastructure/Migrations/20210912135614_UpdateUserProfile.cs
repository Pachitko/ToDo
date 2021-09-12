using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UserProfile_DateOfBirth",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UserProfile_DateOfDeath",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserProfile_MiddleName",
                table: "AspNetUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfile_DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserProfile_DateOfDeath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserProfile_MiddleName",
                table: "AspNetUsers");
        }
    }
}
