namespace Event_Management_System.Models.Views
{
    public class OrganizerEventStatsView
    {
        public int OrganizerId { get; set; }
        public string OrganizerName { get; set; }

        public int TotalEvents { get; set; }
        public int UpcomingEvents { get; set; }
        public int ExpiredEvents { get; set; }

        public int OpenEvents { get; set; }
        public int FullEvents { get; set; }

        public int TotalEnrollments { get; set; }
        public int PromotedEvents { get; set; }
    }
}