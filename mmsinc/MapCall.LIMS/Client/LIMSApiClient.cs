using System;
using System.Net.Http;
using log4net;
using MapCall.LIMS.Configuration;
using MapCall.LIMS.Model.Entities;
using MMSINC.Utilities.APIM;
using Newtonsoft.Json;

namespace MapCall.LIMS.Client
{
    /// <summary>
    /// The LIMS Api Client communicates with the LIMS API - sending and retrieving LIMS entities.
    /// </summary>
    public class LIMSApiClient : IDisposable, ILIMSApiClient
    {
        #region Private Members

        private readonly HttpClient _httpClient;
        private readonly ILog _log;

        #endregion

        #region Constructors

        public LIMSApiClient(
            ILog log, 
            ILIMSClientConfiguration limsConfiguration, 
            IAPIMClientFactory apimClientFactory)
        {
            _log = log;
            _httpClient = apimClientFactory.Build(limsConfiguration);
        }

        #endregion

        #region Exposed Methods

        public Profile[] GetProfiles()
        {
            Profile[] profilesResponse = default;

            var requestUrl = new Uri(_httpClient.BaseAddress, "GetProfiles/All").ToString();
            using (var response = _httpClient.GetAsync(requestUrl).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    profilesResponse = response.Content
                                               .ReadAsAsync<Profile[]>()
                                               .Result;
                }
                else
                {
                    _log.Error($"Unable to make request to LIMS API at: {requestUrl}");
                }

                return profilesResponse;
            }
        }

        public Location CreateLocation(Location location)
        {
            Location[] createLocationResponse = null;

            var serializedLocation = JsonConvert.SerializeObject(location);

            var requestUrl = new Uri(_httpClient.BaseAddress, "CreateLocation").ToString();

            using (var requestContent = new StringContent(JsonConvert.SerializeObject(serializedLocation), System.Text.Encoding.UTF8, "application/json"))
            using (var response = _httpClient.PostAsync(requestUrl, requestContent).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    createLocationResponse = response.Content.ReadAsAsync<Location[]>().Result;
                }
                else
                {
                    _log.Error(
                        $"Unable to make request to LIMS API at: {requestUrl} for the given content: {serializedLocation}");
                }

                return createLocationResponse[0];
            }
        }

        public Location UpdateLocation(Location location)
        {
            Location[] createLocationResponse = null;

            var serializedLocation = JsonConvert.SerializeObject(location);

            var requestUrl = new Uri(_httpClient.BaseAddress, "UpdateLocation").ToString();
            
            using (var requestContent = new StringContent(JsonConvert.SerializeObject(serializedLocation), System.Text.Encoding.UTF8, "application/json"))
            using (var response = _httpClient.PostAsync(requestUrl, requestContent).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    createLocationResponse = response.Content.ReadAsAsync<Location[]>().Result;
                }
                else
                {
                    _log.Error(
                        $"Unable to make request to LIMS API at: {requestUrl} for the given content: {serializedLocation}");
                }

                return createLocationResponse[0];
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        #endregion
    }
}
