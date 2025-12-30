using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Management_System.Migrations
{
    public partial class AddEnrollmentDeleteTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_enrollment_after_delete
                AFTER DELETE ON Enrollments
                BEGIN
                    UPDATE Events
                    SET Status = 0
                    WHERE EventId = OLD.EventId
                      AND Status = 1
                      AND (
                          SELECT COUNT(*)
                          FROM Enrollments
                          WHERE EventId = OLD.EventId
                      ) < (
                          SELECT AvailableSpots
                          FROM Events
                          WHERE EventId = OLD.EventId
                      );
                END;
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS trg_enrollment_after_delete;
                ");
        }
    }
}