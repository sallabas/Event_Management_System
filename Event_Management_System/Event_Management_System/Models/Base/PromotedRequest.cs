using System;
using System.Collections.Generic;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class PromotedRequest
    {
        public int PromotedRequestId { get; set; }

        // Fk properties
        public int EventId { get; set; }
        public int OrganizerId { get; set; }
        
        // Navigation properties
        public Event TargetEvent { get; set; } = null!;
        public Organizer Organizer { get; set; } = null!;

        
        public DateTime RequestDate { get; set; }
        public PromotionStatus Status { get; private set; }

        
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


        public static double MinCostOfPromotion = 10.0;

        // EF core, parameterless ctor
        protected PromotedRequest() { }
        public PromotedRequest(Event targetEvent, Organizer organizer, DateTime requestDate, double amount)
        {
            if (targetEvent == null) throw new ArgumentNullException(nameof(targetEvent), "Target event cannot be null.");
            
            if (organizer == null) throw new ArgumentNullException(nameof(organizer), "organizer cannot be null.");
            
            if (organizer.PaymentDetail == null)
                throw new InvalidOperationException("Organizer must have a PaymentDetail before requesting promotion.");

            if (amount < MinCostOfPromotion)
                throw new ArgumentException($"Amount must be at least {MinCostOfPromotion}.", nameof(amount));

            
            TargetEvent = targetEvent;
            EventId = targetEvent.EventId;
            targetEvent.AddPromotedRequest(this);


            Organizer = organizer;
            OrganizerId = organizer.UserId;
            organizer.AddPromotedRequest(this);
            // if organizer is not monetize we should turn it into monetized after any request !!!!!!
            if (!organizer.IsMonetized)
                organizer.IsMonetized = true;


            RequestDate = requestDate;
            Status = PromotionStatus.Pending;
        }

        public void Confirm()
        {
            Status = PromotionStatus.Confirmed;
        }

        public void Fail()
        {
            Status = PromotionStatus.Failed;
        }

        public void AddTransaction(double amount, DateTime date)
        {
            if (Organizer.PaymentDetail == null)
                throw new InvalidOperationException("Organizer must have a PaymentDetail.");

            var transaction = new Transaction(amount, date, Organizer.PaymentDetail, this);
            Transactions.Add(transaction);
            
        }
        
        public void RemoveTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            Transactions.Remove(transaction);
        }

    }

    public enum PromotionStatus
    {
        Pending,
        Confirmed,
        Failed
    }
}
