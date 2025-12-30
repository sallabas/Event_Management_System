using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Management_System.Migrations
{
    public partial class CreateUpcomingEventsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_upcoming_events AS
                SELECT
                    e.EventId
                FROM Events e
                WHERE
                    e.StartDate >= datetime('now')
                    AND e.EndDate > datetime('now')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS vw_upcoming_events;");
        }
    }
}