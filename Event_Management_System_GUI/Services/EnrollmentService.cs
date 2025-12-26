using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;

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
}