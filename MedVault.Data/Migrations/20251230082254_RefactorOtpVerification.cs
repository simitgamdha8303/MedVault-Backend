using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedVault.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorOtpVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Otp",
                table: "OtpVerifications",
                newName: "OtpHash");

            migrationBuilder.RenameColumn(
                name: "IsVerify",
                table: "OtpVerifications",
                newName: "IsUsed");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "OtpVerifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "OtpVerifications");

            migrationBuilder.RenameColumn(
                name: "OtpHash",
                table: "OtpVerifications",
                newName: "Otp");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "OtpVerifications",
                newName: "IsVerify");
        }
    }
}
