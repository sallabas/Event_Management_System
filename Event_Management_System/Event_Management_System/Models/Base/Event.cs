using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class Event
    {
        public int EventId { get; set; }
        
        // Composite keys 
        public int VenueId { get; set; }
        public int OrganizerId { get; set; }
        
        // Navigation Properties
        public Venue Venue { get; set; } = null!;
        public Organizer Organizer { get; set; } = null!;
        
        //Event Status Property
        public EventStatus Status { get; set; } = EventStatus.Open;
        
        private string _eventTitle = null!;
        private string _description = null!;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _availableSpots;

        
        public string EventTitle
        {
            get => _eventTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be empty.");
                if (value.Length > 200)
                    throw new ArgumentException("Title cannot exceed 200 characters.");
                _eventTitle = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be empty.");
                _description = value;
            }
        }

        // validations taken into constructor due to compilation error!!!!! 
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }

        public int AvailableSpots
        {
            get => _availableSpots;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Available spots must be greater than zero.");
                _availableSpots = value;
            }
        }
        public ICollection<EventCategory> Categories { get; set; } = new List<EventCategory>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<DiscussionComment> Comments { get; set; } = new List<DiscussionComment>();
        public ICollection<PromotedRequest> PromotedRequests { get; set; } = new List<PromotedRequest>();

        

        // parameterless constructor, necessary for EF Core
        protected Event() { }

        public Event(string title, string description, DateTime startDate, DateTime endDate,
                     int availableSpots, Organizer organizer, Venue venue, IEnumerable<EventCategory>? categories = null,
                     IEnumerable<string>? tags = null)
        {
                        
            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.");
            

            EventTitle = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            AvailableSpots = availableSpots;

            Organizer = organizer ?? throw new ArgumentNullException(nameof(organizer));
            Venue = venue ?? throw new ArgumentNullException(nameof(venue));
            
            
            if (categories != null)
            {
                foreach (var cat in categories.Where(c => c != null))
                    AddCategory(cat);
            }

            /*
            if (tags != null)
            {
                foreach (var tag in tags.Where(t => !string.IsNullOrWhiteSpace(t)))
                    AddTag(tag);
            }
            */
            
        }
        
        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment));
            if (!Enrollments.Contains(enrollment))
                Enrollments.Add(enrollment);
            
            if (Enrollments.Any(e => e.UserId == enrollment.UserId))
                throw new InvalidOperationException("User is already enrolled in this event.");
        
        }

        public void RemoveEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment));
            Enrollments.Remove(enrollment);
            
        }

        public void AddComment(DiscussionComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (!Comments.Contains(comment))
                Comments.Add(comment);
            
        }

        public void RemoveComment(DiscussionComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            Comments.Remove(comment);
            

        }

        
        public void AddCategory(EventCategory category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            if (!Categories.Contains(category))
                Categories.Add(category);
        }

        public void RemoveCategory(EventCategory category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            Categories.Remove(category);
  
        }
        

        /*
        public void AddTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag) && !Tags.Contains(tag))
                Tags.Add(tag);
            
            /*if (!string.IsNullOrWhiteSpace(tag) && !_tags.Contains(tag))
                _tags.Add(tag);#1#
        }

        public void RemoveTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return;
            Tags.Remove(tag);
            
            /*
            _tags.Remove(tag);
        #1#
        }
        */
        

        public void AddPromotedRequest(PromotedRequest pr)
        {
            if (pr == null) throw new ArgumentNullException(nameof(pr));
            if (!PromotedRequests.Contains(pr))
                PromotedRequests.Add(pr);

        }

        public void RemovePromotedRequest(PromotedRequest pr)
        {
            if (pr == null) throw new ArgumentNullException(nameof(pr));
            PromotedRequests.Remove(pr); 
        }


        // ADDED BECAUSE OF GUI IMPLEMENTATION, NOT USED NORMALLY !!!!!!!    CHECK THIS LATE !!!!!!!!!!   
        public void AddVenue(Venue venue)
        {
            if (venue == null)
                throw new ArgumentNullException(nameof(venue), "Venue cannot be null.");

            if (Venue != null)
                throw new InvalidOperationException("This event already has a venue assigned.");

            Venue = venue;

            if (!venue.Events.Contains(this))
                venue.Events.Add(this);
        }

        public void RemoveVenue()
        {
            if (Venue == null)
                throw new InvalidOperationException("No venue is assigned to this event.");

            var oldVenue = Venue;

            Venue = null!;      

            oldVenue.Events.Remove(this);
        }
        
        
    }
}
