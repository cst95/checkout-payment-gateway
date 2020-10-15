using System;

namespace PaymentGateway.Data.Models.Entities
{
    public class Payment
    {
        /// <summary>
        /// The unique payment identifier.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The card number (Unique identifier) of the payment card.
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// The CVV number for the payment card.
        /// </summary>
        public int CardCvv { get; set; }
        /// <summary>
        /// The expiry month of the payment card.
        /// </summary>
        public int CardExpiryMonth { get; set; }
        /// <summary>
        /// The expiry year of the payment card.
        /// </summary>
        public int CardExpiryYear { get; set; }
        /// <summary>
        /// The Id of the user that made the payment.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The user that made the payment.
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// The value of the payment.
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The currency of the payment.
        /// </summary>
        public Currency Currency { get; set; }
        /// <summary>
        /// The unique identifier assigned by the acquiring bank.
        /// </summary>
        public string AcquiringBankPaymentId { get; set; }
        /// <summary>
        /// Indicates whether the payment was successful or not.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// The time at which the payment is created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}