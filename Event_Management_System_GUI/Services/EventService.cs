using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;


namespace Event_Management_System_GUI.Services;

public class EventService
{
    private readonly MasDbContext _context;

    public EventService(MasDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllEvents()
    {
        return await _context.Events
            .Include(e => e.Venue)
            .ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .ToListAsync();
    }

    public async Task<Event?> GetEventById(int id)
    {
        return await _context.Events
            .Include(e => e.Venue)
            .ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .FirstOrDefaultAsync(e => e.EventId == id);
    }
}