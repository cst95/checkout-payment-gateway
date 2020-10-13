using System;

namespace PaymentGateway.API.Models.Entities
{
    public class Card
    {
        /// <summary>
        /// The Id of the card.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The 16 digit card number.
        /// </summary>
        public int CardNumber { get; set; }
        /// <summary>
        /// The expiry date of the card.
        /// </summary>
        public DateTime ExpiryDate { get; set; }
        /// <summary>
        /// The 3 digit Cvv number of the card.
        /// </summary>
        public int Cvv { get; set; }
    }
}