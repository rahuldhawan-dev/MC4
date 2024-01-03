using System;
using System.Configuration;
using System.Net.Http;
using MMSINC.Authentication;

namespace MMSINC.Utilities
{
    public class SecureAuthHttpClient : HttpClient
    {
        private readonly ISecureAuthClient _secureAuthClient;
        private string _token;
        private bool _initialized;
        public const string AUTHORIZATION_TOKEN_KEY = "Authorization";

        public SecureAuthHttpClient(ISecureAuthClientFactory secureAuthClientFactory)
        {
            _secureAuthClient = secureAuthClientFactory.Build(new SecureAuthClientConfiguration {
                BaseEndpointUrl = ConfigurationManager.AppSettings["SecureAuth-AuthenticationEndpointBaseUrl"],
                ClientId = ConfigurationManager.AppSettings["SecureAuth-AuthenticationClientId"],
                ClientSecret = ConfigurationManager.AppSettings["SecureAuth-AuthenticationClientSecret"],
                Username = ConfigurationManager.AppSettings["SecureAuth-AuthenticationUsername"],
                Password = ConfigurationManager.AppSettings["SecureAuth-AuthenticationPassword"]
            });
        }

        public virtual void Initialize(SecureAuthHttpClientSettings settings)
        {
            base.BaseAddress = new Uri(settings.Url);
            Timeout = TimeSpan.FromMinutes(settings.Timeout.Value);
            _initialized = true;
        }
    }
}
