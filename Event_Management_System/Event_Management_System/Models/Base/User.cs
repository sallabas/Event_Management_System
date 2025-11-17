using System;
using System.Collections.Generic;
using System.IO;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public abstract class User
    {
       public int UserId { get; set; }

       
        private string _username = null!;
        private string _email = null!;
        private string _password = null!;
        private DateTime _dateOfBirth;


        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                _username = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Invalid email format.");
                _email = value;
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value >= DateTime.Now)
                    throw new ArgumentException("Date of birth must be in the past.");
                _dateOfBirth = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password cannot be empty.");
                _password = value;
            }
        }

        public UserType UserTypes { get; set; }
        
        public PaymentDetail? PaymentDetail { get; set; }
        
        public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();
        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();


        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<DiscussionComment> Comments { get; set; } = new List<DiscussionComment>();


        public ICollection<User> Followers { get; set; } = new HashSet<User>();
        public ICollection<User> Following { get; set; } = new HashSet<User>();
  
        public int FollowersCount => Followers.Count;
        public int FollowingCount => Following.Count;
   
        
        // parameterless constructor
        protected User() { }
        
        protected User(string username, string email, DateTime dateOfBirth, string password)
        {
           // UserId = _idCounter++;
            Username = username;
            Email = email;
            DateOfBirth = dateOfBirth;
            Password = password;
            
           // _users.Add(this);
        }

        
        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment));
            if (!Enrollments.Contains(enrollment)) Enrollments.Add(enrollment);
        }

        public void RemoveEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment));
            Enrollments.Remove(enrollment);
        }

        public void AddComment(DiscussionComment comment)
        {
            
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (!Comments.Contains(comment)) Comments.Add(comment);
        }

        public void RemoveComment(DiscussionComment comment)
        {
            
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            Comments.Remove(comment);
        }
        

        public void Follow(User other)
        {
            
            if (other == null || other == this) return;
            if (!Following.Contains(other))
            {
                Following.Add(other);
                if (!other.Followers.Contains(this))
                    other.Followers.Add(this);
            }
        }

        public void Unfollow(User other)
        {
            if (other == null || other == this) return;
            if (Following.Contains(other))
            {
                Following.Remove(other);
                if (other.Followers.Contains(this))
                    other.Followers.Remove(this);
            }
        }
        
        public void BeFollowedBy(User user)
        {
            
            if (user == null || user == this) return;
            if (!Followers.Contains(user))
            {
                Followers.Add(user);
                if (!user.Following.Contains(this))
                    user.Following.Add(this);
            }
        }

        public void BeUnfollowedBy(User user)
        {
            if (user == null || user == this) return;
            if (Followers.Contains(user))
            {
                Followers.Remove(user);
                if (user.Following.Contains(this))
                    user.Following.Remove(this);
            }
        }
        
        
        
        
        // Message methods 
        // this saves and inform that user get a message, review this. its important !!!
        public void AddSentMessage(Message message)
        {
            
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (!MessagesSent.Contains(message))
                MessagesSent.Add(message);
        }

        public void AddReceivedMessage(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (!MessagesReceived.Contains(message))
                MessagesReceived.Add(message);
        }
        
        // this creates new message, review this. its important !!!!!
        public Message SendMessage(User recipient, string content)
        {
            if (recipient == null)
                throw new ArgumentNullException(nameof(recipient), "Recipient cannot be null.");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Message content cannot be empty.");

            var message = new Message(content, DateTime.Now);
            message.Sender = this;
            message.Receiver = recipient;
            return message;
        }
        
        public void EditSentMessage(int messageId, string newContent)
        {
            
            var message = MessagesSent.FirstOrDefault(m => m.MessageId == messageId);

            if (message == null)
                throw new InvalidOperationException("No message with that ID exists in sent messages.");

            message.EditContent(newContent);
        }

        // .Add .Remove doesnt exist because i set the relation as 0..1 !! do not confuse later aqqq!!!!
        public void SetPaymentDetail(PaymentDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail), "Payment detail cannot be null.");

            if (PaymentDetail != null)
            {
                PaymentDetail.OwnerUser = null;
            }
            
            PaymentDetail = detail;
        }

        public void RemovePaymentDetail()
        {
            PaymentDetail = null;
        }
        
        
    }
    

    [Flags]
    public enum UserType
    {
        Regular = 1,
        Organizer = 2
    }
}
