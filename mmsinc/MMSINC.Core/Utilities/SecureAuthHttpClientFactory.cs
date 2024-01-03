using System.Net.Http;
using StructureMap;

namespace MMSINC.Utilities
{
    public class SecureAuthHttpClientFactory : ISecureAuthHttpClientFactory
    {
        private IContainer _container;

        public HttpClient Build(SecureAuthHttpClientSettings settings)
        {
            var client = _container.GetInstance<SecureAuthHttpClient>();
            client.Initialize(settings);
            return client;
        }

        public SecureAuthHttpClientFactory(IContainer container)
        {
            _container = container;
        }
    }
}
