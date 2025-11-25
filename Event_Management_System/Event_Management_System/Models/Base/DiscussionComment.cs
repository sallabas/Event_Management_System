using Event_Management_System.Models.Base;

[Serializable]
public class DiscussionComment
{
    // pk id for ef core
    public int DiscussionCommentId { get; set; }
    
    // Navigation Properties
    public User Author { get; set; }
    public Event TargetEvent { get; set; }
    
    // FK properties
    public int UserId { get; set; }
    public int TargetEventId { get; set; }
    

    private string _content;

    public string Content
    {
        get => _content;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Comment content cannot be empty.");
            if (value.Length > 1000)
                throw new ArgumentException("Comment content cannot exceed 1000 characters.");
            _content = value;
        }
    }
    public DateTime CreatedAt { get; set; }

    // parameterless constructor for ef core
    protected DiscussionComment() { }

    public DiscussionComment(User user, Event ev, string content)
    {
        
        Author = user ?? throw new ArgumentNullException(nameof(user));
        TargetEvent = ev ?? throw new ArgumentNullException(nameof(ev));
        
        bool isEnrolled = ev.Enrollments.Any(en => en.UserId == user.UserId);

        /*if (!isEnrolled)
            throw new InvalidOperationException(
                "User must be enrolled in the event to write a comment.");*/
        
        UserId = user.UserId;
        TargetEventId = ev.EventId;

        Content = content ?? throw new ArgumentNullException(nameof(content));
        CreatedAt = DateTime.Now;

    }
    
}