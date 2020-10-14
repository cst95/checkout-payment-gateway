namespace PaymentGateway.Models.DTOs
{
    public class ProcessPaymentResponseDto
    {
        public bool Success { get; set; }
        public string PaymentId { get; set; }
    }
}