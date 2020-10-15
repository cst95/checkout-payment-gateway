using System;
using PaymentGateway.Data.Models.Entities;

namespace PaymentGateway.Models.DTOs
{
    public class PaymentDetailsDto
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime PaymentCreatedAt { get; set; }
        public bool Success { get; set; }
        public CardDetailsDto CardDetails { get; set; }
    }
}