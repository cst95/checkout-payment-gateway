namespace PaymentGateway.Interfaces
{
    public interface IProcessedPayment : IUnprocessedPayment
    {
        /// <summary>
        /// The unique identifier supplied by the acquiring bank.
        /// </summary>
        public string AcquiringBankPaymentId { get; set; }
        /// <summary>
        /// Indicates whether the payment was successful or not.
        /// </summary>
        public bool Success { get; set; }
    }
}