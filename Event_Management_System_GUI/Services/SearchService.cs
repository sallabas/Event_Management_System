using Event_Management_System_GUI.Services;
using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System_GUI.Services;

public class SearchService
{
    private readonly UserService _users;
    private readonly EventService _events;
    
    private readonly MasDbContext _context;


    public SearchService(UserService users, EventService events)
    {
        _users = users;
        _events = events;
    }

    public async Task<GlobalSearchResult> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new GlobalSearchResult
            {
                Users = new(),
                Events = new()
            };
        }

        var users = await _users.SearchUsersAsync(query);
        var events = await _events.GetEventsByQueryAsync(query);

        return new GlobalSearchResult
        {
            Users = users,
            Events = events
        };
    }

    
}


public class GlobalSearchResult
{
    public List<User> Users { get; set; } = new();
    public List<Event> Events { get; set; } = new();
}