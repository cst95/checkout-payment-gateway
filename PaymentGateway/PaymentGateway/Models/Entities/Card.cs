using System;

namespace PaymentGateway.API.Models.Entities
{
    public class Card
    {
        /// <summary>
        /// The unique card number.
        /// </summary>
        public int Id { get; set; }
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