// ReSharper disable CheckNamespace
namespace AuthorizeNet
// ReSharper restore CheckNamespace
{
    public interface ICardPresentGateway {
        AuthorizeNet.IGatewayResponse Send(AuthorizeNet.IGatewayRequest request, string description);
    }
}
