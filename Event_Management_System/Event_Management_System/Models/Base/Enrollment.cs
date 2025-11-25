using Event_Management_System.Models.Base;

[Serializable]
public class Enrollment
{

    // Navigation Property
    public User User { get; set; } = null!;
    public Event Event { get; set; } = null!;
    
    // ef core, composite key
    public int UserId { get; set; }
    public int EventId { get; set; }
    
    
    public DateTime EnrollmentDate { get; set; }

    // parameterless ctor for EF core 
    protected Enrollment() { }
    
    public Enrollment(User user, Event ev, DateTime date)
    {
        if (user == null || ev == null)
            throw new ArgumentNullException();
        
        if (date > ev.StartDate)
            throw new ArgumentException("Enrollment date cannot be after the event starts");

        if (date < DateTime.Today)
            throw new ArgumentException("Enrollment date cannot be in the past");
        
        User = user;
        Event = ev;
        
        UserId = user.UserId;
        EventId = ev.EventId;
        
        EnrollmentDate = date;
        
        
    }
    
}