﻿using PaymentGateway.Interfaces;

namespace PaymentGateway.Models
{
    public class ProcessPaymentResult
    {
        /// <summary>
        /// Details of the processed payment.
        /// </summary>
        public IProcessedPayment Payment { get; set; }
    }
}