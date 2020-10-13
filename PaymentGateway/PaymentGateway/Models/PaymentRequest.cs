using System;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Models
{
    public class PaymentRequest : IAcquiringBankRequest
    {
        public int CardNumber { get; set; }
        public DateTime CardExpiryDate { get; set; }
        public int CardCvv { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public User User { get; set; }
    }
}