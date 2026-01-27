using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedVault.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveisActivefromqrShare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "QrShares");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "QrShares",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
