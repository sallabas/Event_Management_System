using System;
using System.Text.Json.Serialization;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class Message
    {
       public int MessageId { get; set; }
       // FK properties
       public int SenderId { get; set; }
       public int ReceiverId { get; set; }

       // Navigation propertoes
       [JsonIgnore]
       public User Sender { get; set; } = null!;
       [JsonIgnore]
       public User Receiver { get; set; } = null!;


       
        private string _content = null!;
        private DateTime _sentDate;
        private DateTime? _viewDate;
        
        public string Content
        {
            get => _content;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Message content cannot be empty.");
                _content = value;
            }
        }

        public DateTime SentDate
        {
            get => _sentDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Sent date cannot be in the future.");
                _sentDate = value;
            }
        }

        public DateTime? ViewDate
        {
            get => _viewDate;
            set => _viewDate = value;
        }

        public bool Received => ViewDate.HasValue;
        public bool IsRead => ViewDate.HasValue;

        
        // reflexive set up - it supports reflexive association !!! WARNING !!!
        public void SetParticipants(User sender, User receiver)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (receiver == null) throw new ArgumentNullException(nameof(receiver));

            Sender = sender;
            Receiver = receiver;

            SenderId = sender.UserId;
            ReceiverId = receiver.UserId;

            if (!sender.MessagesSent.Contains(this))
                sender.MessagesSent.Add(this);

            if (!receiver.MessagesReceived.Contains(this))
                receiver.MessagesReceived.Add(this);
        }

        // parameterless constructor
        protected Message() { }
        public Message(string content, DateTime sentDate)
        {
            Content = content;
            SentDate = sentDate;
        }
        
        public void EditContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("New content cannot be empty.");

            Content = newContent;
        }

    }
}