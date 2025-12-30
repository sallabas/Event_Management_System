using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Management_System.Migrations
{
    public partial class AddEnrollmentAndEventIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UserId_EventId",
                table: "Enrollments",
                columns: new[] { "UserId", "EventId" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Events_Status",
                table: "Events",
                column: "Status"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollments_UserId_EventId",
                table: "Enrollments"
            );

            migrationBuilder.DropIndex(
                name: "IX_Events_Status",
                table: "Events"
            );
        }
    }
}