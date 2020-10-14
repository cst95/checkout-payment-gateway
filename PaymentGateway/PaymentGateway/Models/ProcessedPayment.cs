using System;
using PaymentGateway.Interfaces;
using PaymentGateway.Models.Entities;

namespace PaymentGateway.Models
{
    public class ProcessedPayment : IProcessedPayment
    {
        public string Id { get; set; }
        public string CardNumber { get; set; }
        public int CardCvv { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string AcquiringBankPaymentId { get; set; }
        public bool Success { get; set; }
        public DateTime DateTime { get; set; }
    }
}