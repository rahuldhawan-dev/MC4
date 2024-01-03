using System;
using System.Net.Http;

namespace MMSINC.Utilities
{
    /// <inheritdoc />
    public class HttpClientFactory : IHttpClientFactory
    {
        #region Exposed Methods
        
        /// <inheritdoc />
        public IHttpClient Build()
        {
            return Build(new HttpClientSettings());
        }
        
        /// <inheritdoc />
        public IHttpClient Build(HttpClientSettings settings)
        {
            var httpClient = new HttpClient();

            if (settings.Timeout.HasValue)
            {
                httpClient.Timeout = TimeSpan.FromMinutes(settings.Timeout.Value);
            }

            if (!string.IsNullOrWhiteSpace(settings.Url))
            {
                httpClient.BaseAddress = new Uri(settings.Url);
            }

            if (!string.IsNullOrWhiteSpace(settings.AuthorizationToken))
            {
                httpClient.DefaultRequestHeaders.Add("authorization", settings.AuthorizationToken);
            }

            return new HttpClientWrapper(httpClient);
        }

        #endregion
    }
}
