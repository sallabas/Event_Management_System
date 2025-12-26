using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly MasDbContext _db;
    private readonly CustomAuthStateProvider _authProvider;

    public AuthService(MasDbContext db, AuthenticationStateProvider provider)
    {
        _db = db;
        _authProvider = (CustomAuthStateProvider)provider;
    }

    public async Task<bool> LoginAsync(string usernameOrEmail, string password)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u =>
                (u.Username == usernameOrEmail || u.Email == usernameOrEmail) &&
                u.Password == password);

        if (user == null)
            return false;

        await _authProvider.SetUser(user);

        return true;
    }
    
    public async Task LogoutAsync()
    {
        await _authProvider.SetUser(null);
    }

    public Task<User?> GetCurrentUserAsync()
    {
        return _authProvider.GetCurrentUserAsync();
    }
}
