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
            .AsNoTracking()
            .Include(e => e.Venue)
            .ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .ToListAsync();
    }

    public async Task<Event?> GetEventById(int id)
    {
        return await _context.Events
            .AsNoTracking()
            .Include(e => e.Venue)
            .ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .FirstOrDefaultAsync(e => e.EventId == id);
    }
    
    public async Task AddEvent(Event ev)
    {
        _context.Events.Add(ev);
        await _context.SaveChangesAsync();
    }
    
    // Promoted Event Algorithm
    
    public async Task<List<Event>> GetEventsByQueryAsync(string query)
    {
        query = query?.ToLower() ?? "";
        
        return await _context.Events
            .AsNoTracking()
            .Where(e => e.EventTitle.ToLower().Contains(query.ToLower()))
            .Include(e => e.Venue)
            .ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .ToListAsync();
    }
    public async Task<List<Event>> GetAllEventsRankedAsync()
    {
        var allEvents = await _context.Events
            .AsNoTracking()
            .Include(e => e.Venue).ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .Include(e => e.PromotedRequests)
            .ToListAsync();

        var promotedEvents = allEvents
            .Where(e => e.PromotedRequests.Any(pr => pr.Status == PromotionStatus.Confirmed))
            .OrderByDescending(e =>
                    e.PromotedRequests
                        .Where(pr => pr.Status == PromotionStatus.Confirmed)
                        .Max(pr => pr.RequestDate)           // boost by promotion date
            )
            .ThenByDescending(e => e.Enrollments.Count)  // boost by popularity
            .ToList();

        var promotedIds = promotedEvents.Select(e => e.EventId).ToHashSet();

        var nonPromotedEvents = allEvents
            .Where(e => !promotedIds.Contains(e.EventId))
            .OrderBy(e => e.StartDate)
            .ToList();

        promotedEvents.AddRange(nonPromotedEvents);

        return promotedEvents;
    }


}