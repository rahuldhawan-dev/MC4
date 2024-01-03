using MMSINC.Authentication;

namespace MMSINC.Utilities
{
    public interface ISecureAuthClientFactory
    {
        ISecureAuthClient Build(SecureAuthClientConfiguration config);
    }
}
