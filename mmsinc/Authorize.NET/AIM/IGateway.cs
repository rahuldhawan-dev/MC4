// ReSharper disable CheckNamespace
namespace AuthorizeNet
// ReSharper restore CheckNamespace
{
    public interface IGateway {
        string ApiLogin { get; set; }
        string TransactionKey { get; set; }
		IGatewayResponse Send (IGatewayRequest request);
        IGatewayResponse Send(IGatewayRequest request, string description);
    }
}
