﻿using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Models
{
    public class SavePaymentResult
    {
        /// <summary>
        /// The resulting payment object.
        /// </summary>
        public Payment Payment { get; set; }
    }
}