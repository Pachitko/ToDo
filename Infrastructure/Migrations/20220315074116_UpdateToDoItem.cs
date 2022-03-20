using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateToDoItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ToDoItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ToDoItems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ToDoItems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "Recurrence_Interval",
                table: "ToDoItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Recurrence_Type",
                table: "ToDoItems",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "Recurrence_Interval",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "Recurrence_Type",
                table: "ToDoItems");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ToDoItems",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }
    }
}
