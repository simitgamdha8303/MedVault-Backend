using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedVault.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeinreminders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecurrenceEndDate",
                table: "Reminders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceInterval",
                table: "Reminders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceType",
                table: "Reminders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurrenceEndDate",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "RecurrenceInterval",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "RecurrenceType",
                table: "Reminders");
        }
    }
}
