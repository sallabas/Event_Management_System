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

            // EF Save
            context.Locations.Add(location);
            context.Venues.Add(venue);
            context.Users.AddRange(organizer, regularUser);
            context.PaymentDetails.Add(organizerPayment);
            context.EventCategories.AddRange(musicCategory, raveCategory);
            context.Events.Add(event1);
            context.Enrollments.Add(enrollment);
            context.DiscussionComments.Add(comment);
            context.PromotedRequests.Add(promo);
            context.Messages.AddRange(message1, message2);
            context.SaveChanges();
        }
    }
}
