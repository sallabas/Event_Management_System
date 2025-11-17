using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class Location
    {

        public int LocationId { get; set; }

        private string _country = null!;
        private string _city = null!;
        private string _street = null!;
        private string _postCode = null!;
        
        public ICollection<Venue> Venues { get; set; } = new List<Venue>();

        
        public string Country
        {
            get => _country;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Country cannot be empty.");
                _country = value;
            }
        }

        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("City cannot be empty.");
                _city = value;
            }
        }

        public string Street
        {
            get => _street;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Street cannot be empty.");
                _street = value;
            }
        }

        public string PostCode
        {
            get => _postCode;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Post code cannot be empty.");
                _postCode = value;
            }
        }

        // parameterless constructor
        protected Location() { }
        public Location(string country, string city, string street, string postCode)
        {
            Country = country;
            City = city;
            Street = street;
            PostCode = postCode;
            
        }

        public void AddVenue(Venue venue)
        {
            if (venue == null) throw new ArgumentNullException(nameof(venue));
            if (!Venues.Contains(venue))
                Venues.Add(venue);
        }

        public void RemoveVenue(Venue venue)
        {
            if (venue == null) throw new ArgumentNullException(nameof(venue));
            Venues.Remove(venue);
        }
        
        
    }
}
