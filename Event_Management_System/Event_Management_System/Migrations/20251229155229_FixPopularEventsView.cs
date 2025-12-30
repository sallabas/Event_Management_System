using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class FixPopularEventsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_popular_events;");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_popular_events AS
                SELECT 
                    e.EventId,
                    e.EventTitle,
                    COUNT(en.UserId) AS EnrollmentCount
                FROM Events e
                LEFT JOIN Enrollments en ON e.EventId = en.EventId
                GROUP BY e.EventId, e.EventTitle
                ORDER BY EnrollmentCount DESC;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_popular_events;");
        }
    }
}
