using System;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class Organizer : User
    {
        public ICollection<PromotedRequest> PromotedRequests { get; set; } = new List<PromotedRequest>();
        public ICollection<Event> Events { get; set; } = new List<Event>();

        
        private string _businessDescription;
        private double _earning;

        public string BusinessDescription
        {
            get => _businessDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Business description cannot be empty.");
                _businessDescription = value;
            }
        }

        public bool IsMonetized { get; set; }

        public double Earning
        {
            get => _earning;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Earning cannot be negative.");
                _earning = value;
            }
        }
        
        
        protected Organizer() { }

        public Organizer(string username, string email, DateTime dateOfBirth, string password,
            string businessDescription, bool isMonetized, double earning)
            : base(username, email, dateOfBirth, password)
        {
            BusinessDescription = businessDescription;
            IsMonetized = isMonetized;
            Earning = earning;
            UserTypes = UserType.Organizer;
            
        }

        
        public void AddPromotedRequest(PromotedRequest pr)
        {
            if (pr == null)
                throw new ArgumentNullException(nameof(pr));
            if (!PromotedRequests.Contains(pr))
                PromotedRequests.Add(pr);
        }

        public void RemovePromotedRequest(PromotedRequest pr)
        {
            if (pr == null)
                throw new ArgumentNullException(nameof(pr));
            PromotedRequests.Remove(pr);

        }
        
        public void AddEvent(Event e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            if (!Events.Contains(e))
                Events.Add(e);
        }

        public void RemoveEvent(Event e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            Events.Remove(e);     
        }
        
        
    }
}