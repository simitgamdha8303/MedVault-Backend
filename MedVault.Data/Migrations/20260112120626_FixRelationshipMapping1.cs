using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedVault.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationshipMapping1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalTimelineId1",
                table: "Documents",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MedicalTimelineId1",
                table: "Documents",
                column: "MedicalTimelineId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_MedicalTimelines_MedicalTimelineId1",
                table: "Documents",
                column: "MedicalTimelineId1",
                principalTable: "MedicalTimelines",
                principalColumn: "Id");
        }
    }
}
