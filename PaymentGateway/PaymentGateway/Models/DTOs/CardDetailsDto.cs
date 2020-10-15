namespace PaymentGateway.Models.DTOs
{
    public class CardDetailsDto
    {
        public string MaskedCardNumber { get; set; }
        public int Cvv { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
}