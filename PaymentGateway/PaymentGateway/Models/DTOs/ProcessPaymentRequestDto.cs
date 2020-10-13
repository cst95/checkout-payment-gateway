using System;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Models.DTOs
{
    public class ProcessPaymentRequestDto
    {
        public int CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Cvv { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
    }
}