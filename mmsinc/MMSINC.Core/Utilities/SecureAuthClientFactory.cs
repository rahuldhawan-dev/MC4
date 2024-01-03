using MMSINC.Authentication;

namespace MMSINC.Utilities
{
    public class SecureAuthClientFactory : ISecureAuthClientFactory
    {
        public ISecureAuthClient Build(SecureAuthClientConfiguration config)
        {
            return new SecureAuthClient(config);
        }
    }
}
