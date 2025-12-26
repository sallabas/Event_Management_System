using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;


namespace Event_Management_System_GUI.Services;

public class RegisterResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = "";
}

public class UserService
{
    private readonly MasDbContext _db;

    public UserService(MasDbContext db)
    {
        _db = db;
    }

    public async Task<RegisterResult> RegisterAsync(
        string username,
        string email,
        string password,
        DateTime dateOfBirth,
        string userType)
    {
        var result = new RegisterResult();

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            result.Success = false;
            result.ErrorMessage = "All fields are required.";
            return result;
        }

        bool usernameExists = await _db.Users.AnyAsync(u => u.Username == username);
        if (usernameExists)
        {
            result.Success = false;
            result.ErrorMessage = "Username already taken.";
            return result;
        }

        bool emailExists = await _db.Users.AnyAsync(u => u.Email == email);
        if (emailExists)
        {
            result.Success = false;
            result.ErrorMessage = "Email already registered.";
            return result;
        }

        User newUser;

        if (userType == "Organizer")
        {
            newUser = new Organizer(
                username,
                email,
                dateOfBirth,
                password,
                "New organizer",
                false,
                0.0
            );
        }
        else
        {
            newUser = new RegularUser(
                username,
                email,
                dateOfBirth,
                password,
                "No address"
            );
        }

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        result.Success = true;
        return result;
    }
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<bool> FollowAsync(int followerId, int targetUserId)
    {
        if (followerId == targetUserId)
            return false;

        var follower = await _db.Users
            .Include(u => u.Following)
            .FirstOrDefaultAsync(u => u.UserId == followerId);

        var target = await _db.Users
            .Include(u => u.Followers)
            .FirstOrDefaultAsync(u => u.UserId == targetUserId);

        if (follower == null || target == null)
            return false;

        if (!follower.Following.Contains(target))
        {
            follower.Following.Add(target);
            target.Followers.Add(follower);
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnfollowAsync(int followerId, int targetUserId)
    {
        if (followerId == targetUserId)
            return false;

        var follower = await _db.Users
            .Include(u => u.Following)
            .FirstOrDefaultAsync(u => u.UserId == followerId);

        var target = await _db.Users
            .Include(u => u.Followers)
            .FirstOrDefaultAsync(u => u.UserId == targetUserId);

        if (follower == null || target == null)
            return false;

        if (follower.Following.Contains(target))
        {
            follower.Following.Remove(target);
            target.Followers.Remove(follower);
        }

        await _db.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<User>> SearchUsersAsync(string query)
    {
        query = query?.ToLower() ?? "";
        
        return await _db.Users
            .Where(u => u.Username.ToLower().Contains(query))
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .ToListAsync();

    }
    
    public async Task<bool> UpdateProfileExtendedAsync(
        int userId,
        string email,
        string? address,
        string? businessDescription,
        string? currentPassword,
        string? newPassword)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
            return false;

        // EMAIL 
        user.Email = email;

        // PASSWORD
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
                return false;

            if (user.Password != currentPassword)
                return false;

            user.Password = newPassword;
        }

        if (user is RegularUser regularUser)
        {
            if (!string.IsNullOrWhiteSpace(address))
                regularUser.Address = address;   
        }

        if (user is Organizer organizer)
        {
            if (!string.IsNullOrWhiteSpace(businessDescription))
                organizer.BusinessDescription = businessDescription;
        }

        await _db.SaveChangesAsync();
        return true;
    }



    /*
    public async Task<bool> UpdateProfileAsync(
        int userId,
        string email,
        string? newPassword)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return false;

        user.Email = email;

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            user.Password = newPassword;
        }

        await _db.SaveChangesAsync();
        return true;
    }
    */

    public async Task<string?> UpdateProfileImageAsync(
        int userId,
        IBrowserFile file)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return null;

        var uploadsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "profile"
        );

        Directory.CreateDirectory(uploadsPath);

        var extension = Path.GetExtension(file.Name);
        var fileName = $"user_{userId}{extension}";
        var fullPath = Path.Combine(uploadsPath, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.OpenReadStream(maxAllowedSize: 5_000_000)
            .CopyToAsync(stream);

        user.ProfileImagePath = $"/uploads/profile/{fileName}";
        await _db.SaveChangesAsync();

        return user.ProfileImagePath;
    }
    
    // Follower-Following List 
    public async Task<List<User>> GetFollowersAsync(int userId)
    {
        return await _db.Users
            .Where(u => u.Following.Any(f => f.UserId == userId))
            .ToListAsync();
    }

    public async Task<List<User>> GetFollowingAsync(int userId)
    {
        return await _db.Users
            .Where(u => u.Followers.Any(f => f.UserId == userId))
            .ToListAsync();
    }

    
}
