using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class EventCategory
    {
        public int EventCategoryId { get; set; }

        private string _categoryName = null!;
        private string _categoryDescription = null!;
        
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty.");
                _categoryName = value;
            }
        }

        public string CategoryDescription
        {
            get => _categoryDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category description cannot be empty.");
                _categoryDescription = value;
            }
        }

        public ICollection<Event> Events { get; set; } = new List<Event>();
        
        
        // parameterlss constructor
        protected EventCategory() { }
        public EventCategory(string name, string description)
        {
            CategoryName = name;
            CategoryDescription = description;
        }

        
        public void AddEvent(Event e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!Events.Contains(e))
                Events.Add(e);
        }

        public void RemoveEvent(Event e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (Events.Contains(e))
                Events.Remove(e);    

        }
        
    }
}
