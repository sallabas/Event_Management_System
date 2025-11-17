using System;
using System.Collections.Generic;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class Transaction
    {

        public int TransactionId { get; set; }
        
                
        // fk core, composite key
        public int PaymentDetailId { get; set; }
        public int? PromotedRequestId { get; set; }

        
        // Navigation property
        public PaymentDetail PaymentDetail { get; set; } = null!;
        public PromotedRequest? PromotedRequest { get; set; }
        
        
        
        private double _amount;
        public double Amount
        {
            get => _amount;
            private set
            {
                if (value <= 0 || value > 1_000_000)
                    throw new ArgumentException("Amount must be between 0 and 1,000,000. Do not launder your money xd :) JOKE :)(");
                _amount = value;
            }
        }
        private DateTime _transactionDate;
        public DateTime TransactionDate
        {
            get => _transactionDate;
            private set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Transaction date cannot be in the future.");
                
                _transactionDate = value;
            }
        }
        
        // parameterless constructor
        protected Transaction() { }

        public Transaction(double amount, DateTime transactionDate, PaymentDetail paymentDetail, PromotedRequest? promotedRequest = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            Amount = amount;
            TransactionDate = transactionDate;
            
            PaymentDetail = paymentDetail ?? throw new ArgumentNullException(nameof(paymentDetail));
            PaymentDetailId = paymentDetail.PaymentDetailId;
            
            PromotedRequest = promotedRequest;
            PromotedRequestId = promotedRequest?.PromotedRequestId;
        }

        public void EditTransaction(double newAmount, DateTime newDate)
        {
            Amount = newAmount;
            TransactionDate = newDate;
        }
        
    }
}