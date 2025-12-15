using System.Security.Claims;
using Blazored.LocalStorage;
using Event_Management_System.Models.Base;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private const string STORAGE_KEY = "auth_user";

    private User? _cachedUser;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_cachedUser != null)
            return BuildState(_cachedUser);

        try
        {
            var raw = await _localStorage.GetItemAsync<Dictionary<string, object>>(STORAGE_KEY);

            if (raw == null)
                return BuildState(null);

            User? user;

            var typeValue = raw.ContainsKey("UserTypes") ? raw["UserTypes"]?.ToString() : "1";

            if (typeValue == "2" || typeValue == "Organizer")
            {
                user = await _localStorage.GetItemAsync<Organizer>(STORAGE_KEY);
            }
            else
            {
                user = await _localStorage.GetItemAsync<User>(STORAGE_KEY);
            }

            _cachedUser = user;
            return BuildState(user);
        }
        catch
        {
            return BuildState(null);
        }
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        if (_cachedUser != null)
            return _cachedUser;

        try
        {
            var raw = await _localStorage.GetItemAsync<Dictionary<string, object>>(STORAGE_KEY);

            if (raw == null)
                return null;

            User? user;
            var typeValue = raw.ContainsKey("UserTypes") ? raw["UserTypes"]?.ToString() : "1";

            if (typeValue == "2" || typeValue == "Organizer")
                user = await _localStorage.GetItemAsync<Organizer>(STORAGE_KEY);
            else
                user = await _localStorage.GetItemAsync<User>(STORAGE_KEY);

            _cachedUser = user;
            return _cachedUser;
        }
        catch
        {
            return null;
        }
    }

    public async Task SetUser(User? user)
    {
        _cachedUser = user;

        if (user == null)
        {
            await _localStorage.RemoveItemAsync(STORAGE_KEY);
            NotifyAuthenticationStateChanged(Task.FromResult(BuildState(null)));
            return;
        }

        await _localStorage.SetItemAsync(STORAGE_KEY, user);
        NotifyAuthenticationStateChanged(Task.FromResult(BuildState(user)));
    }

    private AuthenticationState BuildState(User? user)
    {
        if (user == null)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        return new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity(claims, "custom"))
        );
    }
}
