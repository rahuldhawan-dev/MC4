using System.Net.Http;

namespace MMSINC.Utilities
{
    public interface ISecureAuthHttpClientFactory
    {
        HttpClient Build(SecureAuthHttpClientSettings settings);
    }
}
