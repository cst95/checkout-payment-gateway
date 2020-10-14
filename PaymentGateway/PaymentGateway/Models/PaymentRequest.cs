using System;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Models
{
    public class PaymentRequest : IAcquiringBankRequest
    {
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public int CardCvv { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public User User { get; set; }
    }
}