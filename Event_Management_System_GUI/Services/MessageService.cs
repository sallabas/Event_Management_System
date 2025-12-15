using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System_GUI.Services
{
    public class MessageService
    {
        private readonly MasDbContext _db;

        public MessageService(MasDbContext db)
        {
            _db = db;
        }

        public async Task<Message> SendMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new Message(content, DateTime.Now)
            {
                SenderId = senderId,
                ReceiverId = receiverId
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return message;
        }

        public async Task<List<Message>> GetChatAsync(int user1, int user2)
        {
            return await _db.Messages
                .Where(m =>
                    (m.SenderId == user1 && m.ReceiverId == user2) ||
                    (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderBy(m => m.SentDate)
                .ToListAsync();
        }

        public async Task<List<(User User, Message LastMessage)>> GetInboxAsync(int userId)
        {
            var messages = await _db.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .ToListAsync();

            var partners = messages
                .Select(m => m.SenderId == userId ? m.Receiver : m.Sender)
                .Distinct()
                .ToList();

            var inbox = new List<(User User, Message LastMessage)>();

            foreach (var other in partners)
            {
                var lastMsg = messages
                    .Where(m =>
                        (m.SenderId == userId && m.ReceiverId == other.UserId) ||
                        (m.SenderId == other.UserId && m.ReceiverId == userId))
                    .OrderByDescending(m => m.SentDate)
                    .First();

                inbox.Add((other, lastMsg));
            }

            return inbox
                .OrderByDescending(i => i.LastMessage.SentDate)
                .ToList();
        }
    }
}
