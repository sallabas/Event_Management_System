using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System_GUI.Data;

public static class DataSeeder
{
    public static async Task SeedIfEmptyAsync(MasDbContext db)
    {
        if (await db.Events.AnyAsync()) return;

        // Locations
        var loc1 = new Location("Poland", "Warsaw",  "Zamkowa 2", "02-002");
        var loc2 = new Location("Poland", "Krakow",  "Rynek 1",   "02-082");
        var loc3 = new Location("Poland", "Wroclaw", "Opera Hall","02-083");
        var loc4 = new Location("Poland", "Warsaw",  "Smolna 38","02-084");
        var loc5 = new Location("Poland", "Warsaw",  "IT Hall",  "02-085");

        // Venues
        var venue1 = new Venue("Palace of Culture", loc1);
        var venue2 = new Venue("National Bando Arena", loc2);
        var venue3 = new Venue("XXA Theatre", loc3);
        var venue4 = new Venue("Smolna 38", loc4);
        var venue5 = new Venue("University of Warsaw Conference Hall", loc5);

        // Organizer
        var organizer = new Organizer("UNR-DE", "UNR@mail.com", new DateTime(1990,1,1),
                                      "1234", "Unreal Germany", false, 0);

        // Events
        var e1 = new Event("Unreal Warsaw", "Techno Event by Unreal Germany Crew",
                           DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), 50, organizer, venue1);
        var e2 = new Event("National Bando Krakow", "National Bando of Poland",
                           DateTime.Today.AddDays(7), DateTime.Today.AddDays(8), 100, organizer, venue2);
        var e3 = new Event("XXA Theatre", "XXA Theatre",
                           DateTime.Today.AddDays(5), DateTime.Today.AddDays(7), 100, organizer, venue3);
        var e4 = new Event("KASST", "KASST by Smolna 38",
                           DateTime.Today.AddDays(12), DateTime.Today.AddDays(13), 50, organizer, venue4);
        var e5 = new Event("AI Conference", "IT conference",
                           DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), 50, organizer, venue5);
        
        e1.AddVenue(venue1); e2.AddVenue(venue2); e3.AddVenue(venue3); e4.AddVenue(venue4); e5.AddVenue(venue5);

        // Users
        var u1 = new RegularUser("Kemal",    "kemal@mail.com",    new DateTime(2001,5,1),  "pass", "Warsaw");
        var u2 = new RegularUser("Sallabas", "sallabas@mail.com", new DateTime(1999,8,15), "1234", "Warsaw");
        var u3 = new RegularUser("Yigit",    "yigit@mail.com",    new DateTime(1999,8,15), "1234", "Warsaw");
        var u4 = new RegularUser("X01",      "x01@mail.com",      new DateTime(1999,8,15), "1234", "Warsaw");
        var u5 = new RegularUser("X02",      "x02@mail.com",      new DateTime(1999,8,15), "1234", "Warsaw");

        // Enrollments 
        var today = DateTime.Today;
        _ = new Enrollment(u1, e1, today);
        _ = new Enrollment(u2, e1, today);
        _ = new Enrollment(u3, e2, today);
        _ = new Enrollment(u4, e3, today);
        _ = new Enrollment(u5, e4, today);
        _ = new Enrollment(u1, e5, today);
        _ = new Enrollment(u2, e5, today);
        _ = new Enrollment(u3, e5, today);

        // EF save 
        db.AddRange(loc1, loc2, loc3, loc4, loc5);
        db.AddRange(venue1, venue2, venue3, venue4, venue5);
        db.Add(organizer);
        db.AddRange(e1, e2, e3, e4, e5);
        db.AddRange(u1, u2, u3, u4, u5);

        await db.SaveChangesAsync();
    }
}
