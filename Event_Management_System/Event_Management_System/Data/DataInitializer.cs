/*
using System.Collections.Generic;
using System.Linq;
using Event_Management_System.Models.Base;

namespace Event_Management_System.Data
{
    public static class DataInitializer
    {
        public static void Seed(MasDbContext context)
        {
            SeedLocations(context);
            SeedVenues(context);
            SeedCategories(context);
        }

        private static void SeedLocations(MasDbContext context)
        {
            if (context.Locations.Any()) return;

            var locations = new List<Location>
            {
                new("Poland", "Warsaw", "Main St", "00-001"),
                new("Poland", "Krakow", "Old Town", "31-042"),
                new("Poland", "Warsaw", "Smolna 38", "01-045"),
                new("Poland", "Warsaw", "Sielce 12", "02-025"),
                new("Poland", "Warsaw", "Wilanowska 08", "05-025"),
                new("Poland", "Gdansk", "Dluga Street", "80-831"),
                new("Germany", "Berlin", "Alexanderplatz", "10178"),
                new("Czech Republic", "Prague", "Old Town Square", "11000"),
                new("Netherlands", "Amsterdam", "Dam Square", "1012")
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();
        }

        private static void SeedVenues(MasDbContext context)
        {
            if (context.Venues.Any()) return;

            var warsaw = context.Locations.First(l => l.City == "Warsaw");
            var krakow = context.Locations.First(l => l.City == "Krakow");
            var gdansk = context.Locations.First(l => l.City == "Gdansk");
            var berlin = context.Locations.First(l => l.City == "Berlin");
            var prague = context.Locations.First(l => l.City == "Prague");
            var amsterdam = context.Locations.First(l => l.City == "Amsterdam");

            var venues = new List<Venue>
            {
                new("Hala Expo", warsaw),
                new("Smolna", warsaw),
                new("Torwar", warsaw),
                new("XX Coffee Shop", warsaw),
                new("Tauron Arena", krakow),
                new("AmberExpo", gdansk),
                new("Berghain", berlin),
                new("O2 Arena Prague", prague),
                new("Paradiso", amsterdam)
            };

            context.Venues.AddRange(venues);
            context.SaveChanges();
        }

        private static void SeedCategories(MasDbContext context)
        {
            if (context.EventCategories.Any()) return;

            var categories = new List<EventCategory>
            {
                new("Music", "All music events"),
                new("Rave", "Underground techno"),
                new("Festival", "Large scale festivals"),
                new("Workshop", "Hands-on workshops"),
                new("Conference", "Professional conferences"),
                new("Networking", "Business networking events"),
                new("Art", "Art exhibitions and galleries"),
                new("Education", "Educational and academic events"),
                new("Exhibition", "Exhibition events"),
                new("Sport Contest", "Sport competitions"),
                new("MMA", "Mixed Martial Arts"),
                new("Startup", "Startup & entrepreneurship events"),
                new("Other", "Other events")
            };

            context.EventCategories.AddRange(categories);
            context.SaveChanges();
        }
    }
}
*/


using System;
using System.Linq;
using System.Collections.Generic;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System.Data
{
    public static class DataInitializer
    {
        public static void Seed(MasDbContext context)
        {
           if (context.Users.Any()) return; 

            var eventStart = new DateTime(2026, 10, 20, 20, 0, 0);
            var eventEnd = new DateTime(2026, 10, 21, 5, 0, 0);
            var enrollmentDate = new DateTime(2026, 10, 15, 10, 30, 0);
            var promotionRequestDate = new DateTime(2026, 10, 17, 9, 0, 0);

            var location = new Location("Poland", "Warsaw", "Main St", "00-001");
            var venue = new Venue("Hala Expo", location);;

            var organizer = new Organizer("Kemal", "kemal@mail.com", new DateTime(1990, 5, 20), "pass123", "Tech Events", true, 0);
            var regularUser = new RegularUser("Ali", "ali@mail.com", new DateTime(1995, 6, 10), "secret", "Warsaw");

            
            var organizerPayment = new PaymentDetail(organizer,"Kemal Account", "PL60102010260000042270201111");
            organizer.SetPaymentDetail(organizerPayment);

            var musicCategory = new EventCategory("Music", "All music events");
            var raveCategory = new EventCategory("Rave", "Underground techno");

            var event1 = new Event(
                "Rave Night",
                "Join the underground techno rave!",
                eventStart,
                eventEnd,
                100,
                organizer,
                venue,
                new[] { musicCategory, raveCategory },
                new[] { "techno", "nightlife" }
            )
            {
                Venue = venue
            };

            var enrollment = new Enrollment(regularUser, event1, enrollmentDate);
            
            // manual 
            event1.Enrollments.Add(enrollment);
            regularUser.Enrollments.Add(enrollment);
            
            var comment = new DiscussionComment(regularUser, event1, "This looks amazing!");

            var promo = new PromotedRequest(event1, organizer, promotionRequestDate, 15.0);
            promo.Confirm();

            var message1 = organizer.SendMessage(regularUser, "Welcome to our event!");
            var message2 = regularUser.SendMessage(organizer, "Thanks!");

            
            var extraLocations = new List<Location>
            {
                new Location("Poland", "Krakow", "Old Town", "31-042"),
                new Location("Poland", "Warsaw", "Smolna 38", "01-045"),
                new Location("Poland", "Warsaw", "Sielce 12", "02-025"),
                new Location("Poland", "Warsaw", "Wilanowska 08", "05-025"),
                new Location("Poland", "Gdansk", "Dluga Street", "80-831"),
                new Location("Germany", "Berlin", "Alexanderplatz", "10178"),
                new Location("Czech Republic", "Prague", "Old Town Square", "11000"),
                new Location("Netherlands", "Amsterdam", "Dam Square", "1012")
            };


            var extraVenues = new List<Venue>
            {
                new Venue("Tauron Arena", extraLocations.First(l => l.City == "Krakow")),
                new Venue("Smolna", extraLocations.First(l => l.City == "Warsaw")),
                new Venue("Torwar", extraLocations.First(l => l.City == "Warsaw")),
                new Venue("XX Cofee Shop", extraLocations.First(l => l.City == "Warsaw")),



                new Venue("AmberExpo", extraLocations.First(l => l.City == "Gdansk")),
                new Venue("Berghain", extraLocations.First(l => l.City == "Berlin")),
                new Venue("O2 Arena Prague", extraLocations.First(l => l.City == "Prague")),
                new Venue("Paradiso", extraLocations.First(l => l.City == "Amsterdam"))
            };
            
            var extraCategories = new List<EventCategory>
            {
                new EventCategory("Festival", "Large scale festivals"),
                new EventCategory("Workshop", "Hands-on workshops"),
                new EventCategory("Conference", "Professional conferences"),
                new EventCategory("Networking", "Business networking events"),
                new EventCategory("Art", "Art exhibitions and galleries"),
                new EventCategory("Education", "Educational and academic events"),
                new EventCategory("Exhibition", "Exhibiton of an.."),
                new EventCategory("Sport Contest", "Spor Contest"),
                new EventCategory("MMA", "Mixed Martial Art"),
                new EventCategory("Other", "Other.."),

                new EventCategory("Startup", "Startup & entrepreneurship events")
            };
            
            
            // EF Save
            context.Locations.Add(location);
            context.Locations.AddRange(extraLocations);

            context.Venues.Add(venue);
            context.Venues.AddRange(extraVenues);

            context.Users.AddRange(organizer, regularUser);
            context.PaymentDetails.Add(organizerPayment);
            context.EventCategories.AddRange(musicCategory, raveCategory);
            context.EventCategories.AddRange(extraCategories);

            context.Events.Add(event1);
            context.Enrollments.Add(enrollment);
            context.DiscussionComments.Add(comment);
            context.PromotedRequests.Add(promo);
            context.Messages.AddRange(message1, message2);
            context.SaveChanges();
        }
    }
}
