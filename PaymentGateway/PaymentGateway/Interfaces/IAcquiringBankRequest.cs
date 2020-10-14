using PaymentGateway.Data.Models.Entities;

namespace PaymentGateway.Interfaces
{
    public interface IAcquiringBankRequest
    {
        /// <summary>
        /// The unique card number.
        /// </summary>
        string CardNumber { get; set; }
        /// <summary>
        /// The expiry month of the payment card.
        /// </summary>
        public int CardExpiryMonth { get; set; }
        /// <summary>
        /// The expiry year of the payment card.
        /// </summary>
        public int CardExpiryYear { get; set; }
        /// <summary>
        /// The CVV number of the card.
        /// </summary>
        int CardCvv { get; set; }
        /// <summary>
        /// The value of the payment.
        /// </summary>
        decimal Amount { get; set; }
        /// <summary>
        /// The currency of the payment.
        /// </summary>
        Currency Currency { get; set; }
    }
}