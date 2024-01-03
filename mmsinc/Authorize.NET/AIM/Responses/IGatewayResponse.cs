// ReSharper disable CheckNamespace
namespace AuthorizeNet
// ReSharper restore CheckNamespace
{
    public interface IGatewayResponse {
        decimal Amount { get; }
        bool Approved { get; }
        string AuthorizationCode { get; }
        string InvoiceNumber { get; }
        string CardNumber { get; }
        string ResponseCode { get; }
        string Message { get; }
        string TransactionID { get; }
    }
}
