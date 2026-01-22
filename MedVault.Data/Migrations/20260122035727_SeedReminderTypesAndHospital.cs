using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedVault.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedReminderTypesAndHospital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hospitals",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Svastik" },
                    { 2, "Sterling" },
                    { 3, "Apollo" },
                    { 4, "Synergy" },
                    { 5, "Wockhardt" },
                    { 6, "AIIMS" }
                });

            migrationBuilder.InsertData(
                table: "ReminderTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Lab Test" },
                    { 2, "Appointment" },
                    { 3, "Medicine" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hospitals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hospitals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hospitals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Hospitals",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Hospitals",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Hospitals",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ReminderTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReminderTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReminderTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
