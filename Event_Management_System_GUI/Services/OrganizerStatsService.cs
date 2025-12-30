using Microsoft.EntityFrameworkCore;
using Event_Management_System.Data;
using Event_Management_System.Models.Views;

public class OrganizerStatsService
{
    private readonly MasDbContext _context;

    public OrganizerStatsService(MasDbContext context)
    {
        _context = context;
    }

    public async Task<OrganizerEventStatsView?> GetStatsForOrganizerAsync(int organizerId)
    {
        return await _context.OrganizerEventStats
            .FirstOrDefaultAsync(o => o.OrganizerId == organizerId);
    }
}