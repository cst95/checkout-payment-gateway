using System;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Interfaces
{
    public interface IUnprocessedPayment
    {
        /// <summary>
        /// The unique payment identifier.
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// The card number (Unique identifier) of the payment card.
        /// </summary>
        string CardNumber { get; set; }
        /// <summary>
        /// The CVV number for the payment card.
        /// </summary>
        int CardCvv { get; set; }
        /// <summary>
        /// The expiry month of the payment card.
        /// </summary>
        int CardExpiryMonth { get; set; }
        /// <summary>
        /// The expiry year of the payment card.
        /// </summary>
        int CardExpiryYear { get; set; }
        /// <summary>
        /// The Id of the user that made the payment.
        /// </summary>
        string UserId { get; set; }
        /// <summary>
        /// The user that made the payment.
        /// </summary>
        User User { get; set; }
        /// <summary>
        /// The value of the payment.
        /// </summary>
        decimal Amount { get; set; }
        /// <summary>
        /// The currency of the payment.
        /// </summary>
        Currency Currency { get; set; }
        /// <summary>
        /// The time at which the payment is created.
        /// </summary>
        DateTime DateTime { get; set; }
    }
}