using System;
using System.Net;
using System.Net.Http;

namespace MMSINC.Utilities.APIM
{
    /// <summary>
    /// A factory that generates HttpClients for communicating with APIM.
    /// </summary>
    /// <remarks>
    /// APIM is an acronym for SAP API Management. APIM is a single point of entry into any/all on-premises services
    /// such as (SAP itself, LIMS, Open Text, etc)
    /// </remarks>
    /// <seealso cref="MMSINC.Utilities.APIM.IAPIMClientFactory" />
    public class APIMClientFactory : IAPIMClientFactory
    {
        public HttpClient Build(IAPIMClientConfiguration configuration)
        {
            var httpClient = new HttpClient {
                Timeout = TimeSpan.FromMinutes(configuration.TimeoutInMinutes),
                BaseAddress = configuration.ApiUri
            };

            httpClient.DefaultRequestHeaders.Add("ApiKey", configuration.ApiKey);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return httpClient;
        }
    }
}
