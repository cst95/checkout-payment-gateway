namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentsService
    {
        IUnprocessedPayment CreateUnprocessedPayment(IPaymentRequest paymentRequest);
        IProcessedPayment CreateProcessedPayment(IUnprocessedPayment unprocessedPayment, IAcquiringBankResponse acquiringBankResponse);
    }
}