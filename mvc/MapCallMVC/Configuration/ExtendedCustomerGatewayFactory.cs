using AuthorizeNet;
using AuthorizeNet.Utility.NotProvided;

namespace MapCallMVC.Configuration
{
    public class ExtendedCustomerGatewayFactory : IExtendedCustomerGatewayFactory
    {
        #region Private Members

        private readonly IExtendedCustomerGatewayConfiguration _configuration;

        #endregion

        #region Constructors

        public ExtendedCustomerGatewayFactory(IExtendedCustomerGatewayConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Exposed Methods

        public IExtendedCustomerGateway Build()
        {
            return new CustomerGateway(_configuration.APILogin, _configuration.TransactionKey, _configuration.ServiceMode);
        }

        #endregion
    }
}
