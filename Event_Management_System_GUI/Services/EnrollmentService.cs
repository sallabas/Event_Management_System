using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Event_Management_System_GUI.Services;

public class EnrollmentService
{
    private readonly MasDbContext _db;

    public EnrollmentService(MasDbContext db)
    {
        _db = db;
    }

    public async Task<List<Enrollment>> GetMyEnrollmentsAsync(int userId)
    {
        return await _db.Enrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.Event)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync();
    }
    
    public async Task<(bool Success, string Error)> EnrollAsync(int userId, int eventId)
    {
        try
        {
            var user = await _db.Users.FindAsync(userId);
            var ev = await _db.Events.FindAsync(eventId);

            if (user == null || ev == null)
                return (false, "User or event not found.");

            _db.Enrollments.Add(new Enrollment(user, ev, DateTime.Now));
            await _db.SaveChangesAsync();

            return (true, null);
        }
        catch (DbUpdateException ex)
        {
            _db.ChangeTracker.Clear();

            var baseEx = ex.GetBaseException();
            var msg = baseEx.Message ?? ex.Message;

            if (msg.Contains("Event is full", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Event capacity reached", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("capacity", StringComparison.OrdinalIgnoreCase))
            {
                return (false, "This event is already full.");
            }

            return (false, "Enrollment failed due to an unexpected database error.");
        }
    }


}