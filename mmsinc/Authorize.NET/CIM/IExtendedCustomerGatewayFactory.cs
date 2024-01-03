using AuthorizeNet.Utility.NotProvided;

namespace AuthorizeNet
{
    public interface IExtendedCustomerGatewayFactory
    {
        IExtendedCustomerGateway Build();
    }
}