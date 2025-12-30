namespace Event_Management_System.Models.Views
{
    public class PopularEventView
    {
        public int EventId { get; set; }

        public string EventTitle { get; set; } = null!;

        public int EnrollmentCount { get; set; }
    }
}