using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class PaymentDetail
    {

        public int PaymentDetailId { get; set; }

        // FK property
        public int OwnerUserId { get; set; } 
        // Navigation Property
        public User OwnerUser { get; set; } = null!;
        
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        

        private string _accountName = null!;
        public string AccountName
        {
            get => _accountName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Account name cannot be empty.");
                if (value.Length > 100)
                    throw new ArgumentException("Account name cannot exceed 100 characters.");
                _accountName = value;
            }
        }

        private string _iban = null!;
        public string IBAN
        {
            get => _iban;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("IBAN cannot be empty.");

                if (value.Length < 15 || value.Length > 34)
                    throw new ArgumentException("IBAN length must be between 15 and 34 characters.");

                if (!value.All(char.IsLetterOrDigit))
                    throw new ArgumentException("IBAN must contain only letters and digits.");

                _iban = value;
            }
        }

        // parameterless constructor
        protected PaymentDetail() { }
        
        public PaymentDetail(User owner, string accountName, string iban)
        {
            OwnerUser = owner ?? throw new ArgumentNullException(nameof(owner));
            OwnerUserId = owner.UserId;
            
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("Account name cannot be empty.");

            if (string.IsNullOrWhiteSpace(iban))
                throw new ArgumentException("IBAN cannot be empty.");

            AccountName = accountName;
            IBAN = iban;

            owner.SetPaymentDetail(this);
            
        }

        public Transaction AddTransaction(double amount, DateTime date)
        {
            var transaction = new Transaction(amount, date, this);
            Transactions.Add(transaction);
            return transaction;
        }

        public void RemoveTransaction(Transaction transaction)
        {
            if (transaction == null || !Transactions.Contains(transaction))
                throw new ArgumentException("Transaction not found in this payment detail.");

            Transactions.Remove(transaction);
        }
        
        
    }
}