namespace AuthorizeNet
{
    public interface IExtendedCustomerGatewayConfiguration
    {
        string APILogin { get;  }
        string TransactionKey { get; }
        ServiceMode ServiceMode { get; }
    }
}