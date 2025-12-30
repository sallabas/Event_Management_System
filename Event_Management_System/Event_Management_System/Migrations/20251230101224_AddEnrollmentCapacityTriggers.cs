using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Management_System.Migrations
{
    public partial class AddEnrollmentCapacityTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_enrollment_before_insert
                BEFORE INSERT ON Enrollments
                BEGIN
                    SELECT
                        CASE
                            WHEN (SELECT Status FROM Events WHERE EventId = NEW.EventId) = 1
                            THEN RAISE(ABORT, 'Event is full')
                        END;

                    SELECT
                        CASE
                            WHEN (
                                SELECT COUNT(*)
                                FROM Enrollments
                                WHERE EventId = NEW.EventId
                            ) >= (
                                SELECT AvailableSpots
                                FROM Events
                                WHERE EventId = NEW.EventId
                            )
                            THEN RAISE(ABORT, 'Event capacity reached')
                        END;

                    -- If the last insert, change the status to full, this is the important point of the migration
                    UPDATE Events
                    SET Status = 1
                    WHERE EventId = NEW.EventId
                      AND (
                          SELECT COUNT(*)
                          FROM Enrollments
                          WHERE EventId = NEW.EventId
                      ) + 1 >= AvailableSpots;
                END;
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP TRIGGER IF EXISTS trg_enrollment_before_insert;
");
        }
    }
}