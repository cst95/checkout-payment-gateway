using System;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Interfaces
{
    public interface IAcquiringBankRequest
    {
        /// <summary>
        /// The unique card number.
        /// </summary>
        int CardNumber { get; set; }
        /// <summary>
        /// The expiry date of the card.
        /// </summary>
        DateTime CardExpiryDate { get; set; }
        /// <summary>
        /// The 3 digit Cvv number of the card.
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