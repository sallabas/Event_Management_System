using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Management_System.Migrations
{
    public partial class CreateOrganizerStatsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_organizer_event_stats AS
                SELECT
                    o.UserId AS OrganizerId,
                    o.Username AS OrganizerName,

                    COUNT(DISTINCT e.EventId) AS TotalEvents,

                    COALESCE(SUM(
                        CASE
                            WHEN e.StartDate >= datetime('now','localtime')
                             AND e.EndDate   >  datetime('now','localtime')
                            THEN 1 ELSE 0
                        END
                    ),0) AS UpcomingEvents,

                    COALESCE(SUM(
                        CASE
                            WHEN e.EndDate <= datetime('now','localtime')
                            THEN 1 ELSE 0
                        END
                    ),0) AS ExpiredEvents,

                    COALESCE(SUM(
                        CASE WHEN e.Status = 0 THEN 1 ELSE 0 END
                    ),0) AS OpenEvents,

                    COALESCE(SUM(
                        CASE WHEN e.Status = 1 THEN 1 ELSE 0 END
                    ),0) AS FullEvents,

                    COUNT(en.EventId) AS TotalEnrollments,

                    COUNT(DISTINCT CASE
                        WHEN pr.Status = 1 THEN e.EventId
                    END) AS PromotedEvents

                FROM Users o
                LEFT JOIN Events e
                    ON e.OrganizerId = o.UserId
                LEFT JOIN Enrollments en
                    ON en.EventId = e.EventId
                LEFT JOIN PromotedRequests pr
                    ON pr.EventId = e.EventId

                WHERE o.UserTypes = 2
                GROUP BY o.UserId, o.Username;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS vw_organizer_event_stats;");
        }
    }
}