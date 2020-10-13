using System;

namespace PaymentGateway.API.Models.Entities
{
    public class Payment
    {
        /// <summary>
        /// The payment Id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The Id of the card that made the Payment
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// The card that made the payment.
        /// </summary>
        public Card Card { get; set; }
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