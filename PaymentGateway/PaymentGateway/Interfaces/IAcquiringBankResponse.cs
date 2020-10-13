namespace PaymentGateway.API.Interfaces
{
    public interface IAcquiringBankResponse
    {
        /// <summary>
        /// The unique identifier issued by the acquiring bank.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Indicates whether the payment was successful or not.
        /// </summary>
        public bool Success { get; set; }
    }
}