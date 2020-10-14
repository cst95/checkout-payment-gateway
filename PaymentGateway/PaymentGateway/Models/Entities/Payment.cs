using System;

namespace PaymentGateway.API.Models.Entities
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
        /// The 3 digit CVV number for the payment card.
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
        /// The User that made the payment.
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
        /// Indicates whether the payment was successful or not.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// The time at which the payment occured.
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}