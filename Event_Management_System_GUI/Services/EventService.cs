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
        _context.Attach(ev.Organizer);
        _context.Attach(ev.Venue);

        bool venueBusy = await _context.Events.AnyAsync(e =>
            e.Venue.VenueId == ev.Venue.VenueId &&
            e.EventId != ev.EventId &&

            e.StartDate < ev.EndDate &&
            e.EndDate > ev.StartDate
        );

        if (venueBusy)
        {
            throw new InvalidOperationException(
                "Another event is already scheduled at this venue during the selected time."
            );
        }

        var categoryIds = ev.Categories
            .Select(c => c.EventCategoryId)
            .ToList();

        ev.Categories.Clear();

        var trackedCategories = await _context.EventCategories
            .Where(c => categoryIds.Contains(c.EventCategoryId))
            .ToListAsync();

        foreach (var cat in trackedCategories)
        {
            ev.Categories.Add(cat);
        }

        // 3️⃣ Save
        _context.Events.Add(ev);
        await _context.SaveChangesAsync();
    }

    
    /*
    public async Task AddEvent(Event ev)
    {
        _context.Attach(ev.Organizer);
        _context.Attach(ev.Venue);

        var categoryIds = ev.Categories
            .Select(c => c.EventCategoryId)
            .ToList();

        ev.Categories.Clear();

        var trackedCategories = await _context.EventCategories
            .Where(c => categoryIds.Contains(c.EventCategoryId))
            .ToListAsync();

        foreach (var cat in trackedCategories)
        {
            ev.Categories.Add(cat);
        }

        _context.Events.Add(ev);
        await _context.SaveChangesAsync();
    }
    */

    
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
        var now = DateTime.Now;

        var allEvents = await _context.Events
            .AsNoTracking()
            .Include(e => e.Venue).ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .Include(e => e.PromotedRequests)
            .Where(e => e.EndDate > now)
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
    
    public async Task UpdateEvent(Event updatedEvent)
    {
        var existingEvent = await _context.Events
            .Include(e => e.Categories)
            .FirstOrDefaultAsync(e => e.EventId == updatedEvent.EventId);

        if (existingEvent == null)
            throw new InvalidOperationException("Event not found.");

        if (DateTime.Now >= existingEvent.StartDate)
            throw new InvalidOperationException("Started events cannot be edited.");

        if (existingEvent.OrganizerId != updatedEvent.OrganizerId)
            throw new InvalidOperationException("You are not allowed to edit this event.");

        existingEvent.EventTitle = updatedEvent.EventTitle;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.StartDate = updatedEvent.StartDate;
        existingEvent.EndDate = updatedEvent.EndDate;
        existingEvent.AvailableSpots = updatedEvent.AvailableSpots;

        existingEvent.VenueId = updatedEvent.VenueId;

        existingEvent.Categories.Clear();
        foreach (var cat in updatedEvent.Categories)
        {
            existingEvent.Categories.Add(cat);
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteEvent(int eventId, int organizerId)
    {
        var ev = await _context.Events
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (ev == null)
            throw new InvalidOperationException("Event not found.");

        if (DateTime.Now >= ev.StartDate)
            throw new InvalidOperationException("Started events cannot be deleted.");

        if (ev.OrganizerId != organizerId)
            throw new InvalidOperationException("You are not allowed to delete this event.");

        _context.Events.Remove(ev);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Event>> GetFilteredEventsAsync(
        int? categoryId,
        DateTime? startDate,
        DateTime? endDate)
    {
        var now = DateTime.Now;

        var query = _context.Events
            .AsNoTracking()
            .Include(e => e.Venue).ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .Where(e => e.EndDate > now)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(e =>
                e.Categories.Any(c => c.EventCategoryId == categoryId.Value));
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.StartDate >= startDate.Value);
        }
        
        if (endDate.HasValue)
        {
            var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(e => e.StartDate <= endOfDay);
        }


        return await query
            .OrderBy(e => e.StartDate)
            .ToListAsync();
    }

    // View PopularEvents
    public async Task<List<Event>> GetPopularEventsAsync()
    {
        var popularIds = await _context.PopularEvents
            .OrderByDescending(p => p.EnrollmentCount)
            .Select(p => p.EventId)
            .ToListAsync();

        if (!popularIds.Any())
            return new List<Event>();

        var events = await _context.Events
            .AsNoTracking()
            .Include(e => e.Venue).ThenInclude(v => v.Location)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            // .Include(e => e.PromotedRequests)
            .Where(e => popularIds.Contains(e.EventId))
            .ToListAsync();

        return events
            .OrderBy(e => popularIds.IndexOf(e.EventId))
            .ToList();
    }
    
    public async Task<List<Event>> GetUpcomingEventsAsync()
    {
        return await GetUpcomingEventsFromViewAsync();
    }
    
    public async Task<List<Event>> GetUpcomingEventsFromViewAsync()
    {
        return await _context.Events
            .Where(e =>
                _context.UpcomingEvents.Any(v => v.EventId == e.EventId)
            )
            .Include(e => e.Venue).ThenInclude(v => v.Location)
            .Include(e => e.Organizer)
            .Include(e => e.Categories)
            .Include(e => e.Enrollments)
            .Include(e => e.PromotedRequests)
            .OrderBy(e => e.StartDate)
            .ToListAsync();
    }



}