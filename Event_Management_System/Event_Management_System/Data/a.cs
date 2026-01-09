using System;
using System.Collections.Generic;
using System.Linq;
using Event_Management_System.Models.Base;

namespace Event_Management_System.Data
{
    public static class a
    {
        public static void Seed(MasDbContext context)
        {
            SeedLocations(context);
            SeedVenues(context);
            SeedCategories(context);
            SeedUsers(context);
            SeedEvents(context);
            SeedEnrollments(context);
            SeedPromotions(context);
            SeedMessages(context);

            context.SaveChanges();
        }

        // -------------------- LOCATIONS --------------------
        private static void SeedLocations(MasDbContext context)
        {
            if (context.Locations.Any()) return;

            context.Locations.AddRange(
                new Location("Poland", "Krakow", "Old Town", "31-042"),
                new Location("Poland", "Warsaw", "Smolna 38", "01-045"),
                new Location("Poland", "Warsaw", "Sielce 12", "02-025"),
                new Location("Poland", "Warsaw", "Wilanowska 08", "05-025"),
                new Location("Poland", "Gdansk", "Dluga Street", "80-831"),
                new Location("Germany", "Berlin", "Alexanderplatz", "10178"),
                new Location("Czech Republic", "Prague", "Old Town Square", "11000"),
                new Location("Netherlands", "Amsterdam", "Dam Square", "1012")
            );
        }

        // -------------------- VENUES --------------------
        private static void SeedVenues(MasDbContext context)
        {
            if (context.Venues.Any()) return;

            var krakow    = context.Locations.First(l => l.City == "Krakow");
            var warsaw1   = context.Locations.First(l => l.City == "Warsaw" && l.Street == "Smolna 38");
            var warsaw2   = context.Locations.First(l => l.City == "Warsaw" && l.Street == "Sielce 12");
            var warsaw3   = context.Locations.First(l => l.City == "Warsaw" && l.Street == "Wilanowska 08");
            var gdansk    = context.Locations.First(l => l.City == "Gdansk");
            var berlin    = context.Locations.First(l => l.City == "Berlin");
            var prague    = context.Locations.First(l => l.City == "Prague");
            var amsterdam = context.Locations.First(l => l.City == "Amsterdam");

            context.Venues.AddRange(
                new Venue("Tauron Arena", krakow),
                new Venue("Smolna", warsaw1),
                new Venue("Torwar", warsaw2),
                new Venue("XX Coffee Shop", warsaw3),
                new Venue("AmberExpo", gdansk),
                new Venue("Berghain", berlin),
                new Venue("O2 Arena Prague", prague),
                new Venue("Paradiso", amsterdam)
            );
        }


        // -------------------- CATEGORIES --------------------
        private static void SeedCategories(MasDbContext context)
        {
            if (context.EventCategories.Any()) return;

            context.EventCategories.AddRange(
                new EventCategory("Music", "Music events"),
                new EventCategory("Rave", "Underground techno"),
                new EventCategory("Festival", "Large scale festivals"),
                new EventCategory("Workshop", "Hands-on workshops"),
                new EventCategory("Conference", "Professional conferences"),
                new EventCategory("Networking", "Business networking events"),
                new EventCategory("Art", "Art exhibitions and galleries"),
                new EventCategory("Education", "Educational and academic events"),
                new EventCategory("Exhibition", "Exhibiton of an.."),
                new EventCategory("Sport Contest", "Spor Contest"),
                new EventCategory("MMA", "Mixed Martial Art"),
                new EventCategory("Startup", "Startup & entrepreneurship events"),
                new EventCategory("Other", "Other..")
            );
        }

        // -------------------- USERS --------------------
        private static void SeedUsers(MasDbContext context)
        {
            if (context.Users.Any()) return;

            var organizer1 = new Organizer(
                "Kemal",
                "kemal@gmail.com",
                new DateTime(1990, 5, 20),
                "kemal",
                "Tech Events",
                false,
                0
            );

            var organizer2 = new Organizer(
                "Yigit",
                "yigit@gmail.com",
                new DateTime(1988, 3, 12),
                "secret",
                "Music Organization",
                false,
                0
            );
            
            var organizer3 = new Organizer(
                "Sallabas",
                "sallabas@gmail.com",
                new DateTime(2000, 4, 2),
                "sallabas",
                "SXA Organization",
                false,
                0
            );
            
            var organizer4 = new Organizer(
                "rufusdusol",
                "rufus@gmail.com",
                new DateTime(1998, 10, 29),
                "rufus",
                "Rufus Du Sol",
                false,
                0
            );
            
            var organizer5 = new Organizer(
                "Duran",
                "duran@gmail.com",
                new DateTime(1968, 10, 29),
                "duran",
                "Art Exhibitions",
                false,
                0
            );

            organizer1.SetPaymentDetail(
                new PaymentDetail(organizer1, "Kemal Sallabas", "PL60102010260000042270201111")
            );

            organizer2.SetPaymentDetail(
                new PaymentDetail(organizer2, "Yigit Sallabas", "PL89370400440532013000")
            );
            
            organizer3.SetPaymentDetail(
                new PaymentDetail(organizer3, "Sallabas CO.", "PL523896592356823672876")
            );
            
            organizer4.SetPaymentDetail(
                new PaymentDetail(organizer4, "James Du Sol", "PL8925837328563926726234")
            );

            organizer5.SetPaymentDetail(
                new PaymentDetail(organizer5, "Duran Sallabas", "PL234532010260000042212794311")
            );
            
            context.Users.AddRange(
                organizer1,
                organizer2,
                organizer3,
                organizer4,
                organizer5,
                new RegularUser("Ali", "ali@gmail.com", new DateTime(1996, 6, 10), "123", "Warsaw"),
                new RegularUser("Vika", "vika@gmail.com", new DateTime(1999, 12, 1), "vika123", "Berlin"),
                new RegularUser("Quaresma", "q7@gmail.com", new DateTime(1987, 11, 2), "quaresma07", "Krakow"),
                new RegularUser("Melek", "melek@gmail.com", new DateTime(1969, 2, 19), "melek", "Berlin"),
                new RegularUser("Derin", "derin@gmail.com", new DateTime(2003, 8, 1), "derin", "Istanbul"),
                new RegularUser("mercan", "mercan@gmail.com", new DateTime(2005, 2, 26), "mercan123", "Warsaw"),
                new RegularUser("Dan", "dan@gmail.com", new DateTime(1996, 5, 10), "dan123", "Warsaw"),
                new RegularUser("Bilal", "bilal@gmail.com", new DateTime(2001, 4, 10), "bilal123", "Warsaw"),
                new RegularUser("Kebab", "kebab@gmail.com", new DateTime(2006, 6, 10), "kebab123", "Istanbul")
            );
        }

        // -------------------- EVENTS --------------------
        private static void SeedEvents(MasDbContext context)
        {
            if (context.Events.Any()) return;
            
            var kemal     = context.Organizers.First(o => o.Email == "kemal@gmail.com");
            var yigit     = context.Organizers.First(o => o.Email == "yigit@gmail.com");
            var sallabas  = context.Organizers.First(o => o.Email == "sallabas@gmail.com");
            var rufus     = context.Organizers.First(o => o.Email == "rufus@gmail.com");
            var duran     = context.Organizers.First(o => o.Email == "duran@gmail.com");

            var smolna    = context.Venues.First(v => v.VenueName == "Smolna");
            var torwar    = context.Venues.First(v => v.VenueName == "Torwar");
            var taurona   = context.Venues.First(v => v.VenueName == "Tauron Arena");
            var berghain  = context.Venues.First(v => v.VenueName == "Berghain");
            var amberexpo = context.Venues.First(v => v.VenueName == "AmberExpo");
            var paradiso  = context.Venues.First(v => v.VenueName == "Paradiso");

            var music      = context.EventCategories.First(c => c.CategoryName == "Music");
            var rave       = context.EventCategories.First(c => c.CategoryName == "Rave");
            var conference = context.EventCategories.First(c => c.CategoryName == "Conference");
            var workshop   = context.EventCategories.First(c => c.CategoryName == "Workshop");
            var art        = context.EventCategories.First(c => c.CategoryName == "Art");

            context.Events.AddRange(

                // kemal
                new Event("Warsaw Techno Night", "Underground techno all night",
                    DateTime.Now.AddDays(10), DateTime.Now.AddDays(11), 250,
                    kemal, smolna, new[] { music, rave }),

                new Event("Future of AI Conference", "AI, data and future tech",
                    DateTime.Now.AddDays(30), DateTime.Now.AddDays(31), 400,
                    kemal, torwar, new[] { conference }),

                new Event("Startup Pitch Day", "Pitch your startup to investors",
                    DateTime.Now.AddDays(45), DateTime.Now.AddDays(46), 300,
                    kemal, torwar, new[] { conference }),

                // yigit
                new Event("Electronic Music Festival", "Electronic & house music",
                    DateTime.Now.AddDays(15), DateTime.Now.AddDays(17), 800,
                    yigit, taurona, new[] { music }),

                new Event("Deep House Night", "Deep & melodic house vibes",
                    DateTime.Now.AddDays(25), DateTime.Now.AddDays(26), 350,
                    yigit, smolna, new[] { music }),

                new Event("Live DJ Showcase", "Local & international DJs",
                    DateTime.Now.AddDays(35), DateTime.Now.AddDays(36), 300,
                    yigit, paradiso, new[] { music }),

                // sallabas
                new Event("SXA Coding Workshop", "Hands-on fullstack workshop",
                    DateTime.Now.AddDays(12), DateTime.Now.AddDays(13), 120,
                    sallabas, amberexpo, new[] { workshop }),

                new Event("Game Dev Bootcamp", "Unity & Unreal basics",
                    DateTime.Now.AddDays(22), DateTime.Now.AddDays(24), 150,
                    sallabas, amberexpo, new[] { workshop }),

                new Event("Cybersecurity Fundamentals", "Intro to ethical hacking",
                    DateTime.Now.AddDays(40), DateTime.Now.AddDays(41), 200,
                    sallabas, torwar, new[] { conference }),

                // rufus du sol
                new Event("RÜFÜS DU SOL Live", "World tour live performance",
                    DateTime.Now.AddDays(18), DateTime.Now.AddDays(19), 1200,
                    rufus, berghain, new[] { music }),

                new Event("Sunset Electronic Session", "Melodic sunset vibes",
                    DateTime.Now.AddDays(28), DateTime.Now.AddDays(29), 600,
                    rufus, paradiso, new[] { music }),

                new Event("Afterparty Berlin", "Late night afterparty",
                    DateTime.Now.AddDays(29), DateTime.Now.AddDays(30), 500,
                    rufus, berghain, new[] { rave }),

                // duran
                new Event("Modern Art Exhibition", "Contemporary art showcase",
                    DateTime.Now.AddDays(8), DateTime.Now.AddDays(20), 1000,
                    duran, amberexpo, new[] { art }),

                new Event("Digital Art & AI", "AI-generated art installations",
                    DateTime.Now.AddDays(26), DateTime.Now.AddDays(38), 900,
                    duran, amberexpo, new[] { art }),

                new Event("Photography & Light", "Visual storytelling exhibition",
                    DateTime.Now.AddDays(42), DateTime.Now.AddDays(55), 700,
                    duran, taurona, new[] { art })
            );
        }


        // -------------------- ENROLLMENTS --------------------
        private static void SeedEnrollments(MasDbContext context)
        {
            if (context.Enrollments.Any()) return;

            var users = context.RegularUsers.ToList();
            var events = context.Events
                .OrderBy(e => e.StartDate)
                .ToList();

            DateTime enrollDate = DateTime.Today.AddDays(1);

            // event 1
            context.Enrollments.AddRange(
                new Enrollment(users[0], events[0], enrollDate),
                new Enrollment(users[1], events[0], enrollDate),
                new Enrollment(users[2], events[0], enrollDate),
                new Enrollment(users[3], events[0], enrollDate)
            );

            // event 2
            context.Enrollments.AddRange(
                new Enrollment(users[4], events[1], enrollDate),
                new Enrollment(users[5], events[1], enrollDate),
                new Enrollment(users[6], events[1], enrollDate)
            );

            // event 3
            context.Enrollments.AddRange(
                new Enrollment(users[7], events[2], enrollDate),
                new Enrollment(users[8], events[2], enrollDate)
            );

            // event 4
            context.Enrollments.AddRange(
                new Enrollment(users[0], events[3], enrollDate),
                new Enrollment(users[1], events[3], enrollDate),
                new Enrollment(users[2], events[3], enrollDate),
                new Enrollment(users[3], events[3], enrollDate),
                new Enrollment(users[4], events[3], enrollDate),
                new Enrollment(users[5], events[3], enrollDate)
            );

            // event 5
            context.Enrollments.AddRange(
                new Enrollment(users[6], events[4], enrollDate),
                new Enrollment(users[7], events[4], enrollDate)
            );

            // event 6
            context.Enrollments.Add(
                new Enrollment(users[8], events[5], enrollDate)
            );

            // event 7
            context.Enrollments.AddRange(
                new Enrollment(users[0], events[6], enrollDate),
                new Enrollment(users[1], events[6], enrollDate),
                new Enrollment(users[2], events[6], enrollDate)
            );

            // event 8
            context.Enrollments.AddRange(
                new Enrollment(users[3], events[7], enrollDate),
                new Enrollment(users[4], events[7], enrollDate)
            );

            // event 9
            context.Enrollments.Add(
                new Enrollment(users[5], events[8], enrollDate)
            );

            // event 10
            context.Enrollments.AddRange(
                new Enrollment(users[0], events[9], enrollDate),
                new Enrollment(users[1], events[9], enrollDate),
                new Enrollment(users[2], events[9], enrollDate),
                new Enrollment(users[3], events[9], enrollDate),
                new Enrollment(users[4], events[9], enrollDate),
                new Enrollment(users[5], events[9], enrollDate),
                new Enrollment(users[6], events[9], enrollDate)
            );

            // event 11
            context.Enrollments.AddRange(
                new Enrollment(users[7], events[10], enrollDate),
                new Enrollment(users[8], events[10], enrollDate)
            );

            // event 12
            context.Enrollments.Add(
                new Enrollment(users[1], events[11], enrollDate)
            );

            // event 13
            context.Enrollments.AddRange(
                new Enrollment(users[2], events[12], enrollDate),
                new Enrollment(users[3], events[12], enrollDate),
                new Enrollment(users[4], events[12], enrollDate),
                new Enrollment(users[5], events[12], enrollDate)
            );
        }


        // -------------------- PROMOTIONS --------------------
        private static void SeedPromotions(MasDbContext context)
        {
            if (context.PromotedRequests.Any()) return;

            var kemal    = context.Organizers.First(o => o.Email == "kemal@gmail.com");
            var yigit    = context.Organizers.First(o => o.Email == "yigit@gmail.com");
            var rufus    = context.Organizers.First(o => o.Email == "rufus@gmail.com");

            var raveEvent        = context.Events.First(e => e.EventTitle == "Warsaw Techno Night");
            var festivalEvent    = context.Events.First(e => e.EventTitle == "Electronic Music Festival");
            var liveConcertEvent = context.Events.First(e => e.EventTitle == "RÜFÜS DU SOL Live");

            // promotion 1
            var promo1 = new PromotedRequest(
                raveEvent,
                kemal,
                DateTime.Today.AddDays(-3),
                20
            );
            promo1.Confirm();
            promo1.AddTransaction(20, DateTime.Today.AddDays(-2));

            // promotion 2
            var promo2 = new PromotedRequest(
                festivalEvent,
                yigit,
                DateTime.Today.AddDays(-5),
                35
            );
            promo2.Confirm();
            promo2.AddTransaction(35, DateTime.Today.AddDays(-4));

            // promotion 3
            var promo3 = new PromotedRequest(
                liveConcertEvent,
                rufus,
                DateTime.Today.AddDays(-7),
                50
            );
            promo3.Confirm();
            promo3.AddTransaction(50, DateTime.Today.AddDays(-6));

            context.PromotedRequests.AddRange(promo1, promo2, promo3);
        }


        // -------------------- MESSAGES --------------------
        private static void SeedMessages(MasDbContext context)
        {
            if (context.Messages.Any()) return;

            var u1 = context.Users.First();
            var u2 = context.Users.Skip(1).First();

            context.Messages.AddRange(
                u1.SendMessage(u2, "Welcome to the platform!"),
                u2.SendMessage(u1, "Thanks!")
            );
        }
    }
}
