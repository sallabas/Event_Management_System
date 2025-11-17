using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class Venue
    {
        // empty getter for EF core
        
        public int VenueId { get; set; }
        
        // fk property 
        public int LocationId { get; set; }


        
        private string _venueName;
        public string VenueName
        {
            get => _venueName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Venue name cannot be empty.");
                _venueName = value;
            }
        }

        private Location _location = null!;
        public Location Location
        {
            get => _location;
            set
            {
                _location = value ?? throw new ArgumentNullException(nameof(Location), "Location cannot be null.");
                LocationId = value.LocationId;
            }
        }

        public ICollection<Event> Events { get; set; } = new List<Event>();


        // parameterless constructor
        protected Venue() { }
        public Venue(string venueName, Location location)
        {
            VenueName = venueName;

            Location = location;
            LocationId = location.LocationId;
            
        }

        public void AddEvent(Event e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!Events.Contains(e)) Events.Add(e);
        }

        public void RemoveEvent(Event e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            Events.Remove(e);
        }
    }
}