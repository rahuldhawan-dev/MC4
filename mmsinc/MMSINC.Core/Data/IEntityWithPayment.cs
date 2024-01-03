using System;

namespace MMSINC.Data
{
    public interface IEntityWithPayment
    {
        string PaymentTransactionId { get; set; }
        string PaymentAuthorizationCode { get; set; }
        DateTime? PaymentReceivedAt { get; set; }
        string PaymentProfileId { get; set; }
        decimal? TotalCharged { get; set; }
        string InvoiceNumber { get; }
    }
}
