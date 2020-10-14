namespace PaymentGateway.Domain.Interfaces
{
    public interface IAcquiringBankResponse
    {
        /// <summary>
        /// The unique identifier issued by the acquiring bank.
        /// </summary>
        public string PaymentId { get; set; }
        /// <summary>
        /// Indicates whether the payment was successful or not.
        /// </summary>
        public bool Success { get; set; }
    }
}