using System.ComponentModel.DataAnnotations;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Models.DTOs
{
    // TODO: Implement validation of this class
    public class ProcessPaymentRequestDto
    {
        [CreditCard]
        public string CardNumber { get; set; }
        
        [Range(1,12)]
        public int CardExpiryMonth { get; set; }
        
        public int CardExpiryYear { get; set; }
        
        [Range(100, 9999, ErrorMessage = "CVV must be either 3 or 4 digits.")]
        public int Cvv { get; set; }
        
  
        public decimal Amount { get; set; }
        
        [EnumDataType(typeof(Currency))]
        public Currency Currency { get; set; }
    }
}