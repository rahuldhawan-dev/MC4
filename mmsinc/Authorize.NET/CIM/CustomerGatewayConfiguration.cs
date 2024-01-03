using System.Configuration;
using AuthorizeNet.Utility.NotProvided;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Common;

namespace AuthorizeNet
{
    public class CustomerGatewayConfiguration : IExtendedCustomerGatewayConfiguration
    {
        public string APILogin => ConfigurationManager.AppSettings.EnsureValue(MissingMethods.AppSettingsKeys.AUTHORIZE_NET_LOGIN_ID);
        public string TransactionKey => ConfigurationManager.AppSettings.EnsureValue(MissingMethods.AppSettingsKeys.AUTHORIZE_NET_TX_KEY);
        public ServiceMode ServiceMode => HttpApplicationBase.IsProduction ? ServiceMode.Live : ServiceMode.Test;
    }
}