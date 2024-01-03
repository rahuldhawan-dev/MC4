using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace MMSINC.Authentication
{
    public interface ISecureAuthClient : IDisposable
    {
        SecureAuthAccessToken GetAccessToken();
    }

    /// <summary>
    /// SecureAuthClient instances should be used as a singleton or otherwise long-lived
    /// object when dealing with the same endpoints. HttpClient is used internally which is
    /// not meant to be instantiated repeatedly in a short time frame.
    /// </summary>
    public class SecureAuthClient : ISecureAuthClient
    {
        #region Fields

        private HttpClient _httpClient;
        private SecureAuthClientConfiguration _config;

        #endregion

        #region Constructor

        public SecureAuthClient(SecureAuthClientConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
            // TODO: Init HttpClient with config.
            _httpClient.BaseAddress = new Uri(config.BaseEndpointUrl);
        }

        #endregion

        #region Private Methods

        private static T DeserializeJson<T>(string json)
        {
            using (var reader = new System.IO.StringReader(json))
            using (var jsonReader = new Newtonsoft.Json.JsonTextReader(reader))
            {
                return new Newtonsoft.Json.JsonSerializer().Deserialize<T>(jsonReader);
            }
        }

        #endregion

        #region Public Methods

        // This method exists for unit testing only.
        // There is no other way to test that this content
        // gets created correctly because it's immediately converted
        // into a byte array.
        internal Dictionary<string, string> GetRequestContentForGetAccessToken()
        {
            var contentValues = new Dictionary<string, string>();
            contentValues["grant_type"] =
                "password"; // NOTE: This is not a placeholder, it's not a password, the value just literally needs to be "password"
            contentValues["scope"] = "openid"; // NOTE: Also a hardcoded value that just needs to be "openid"
            contentValues["client_id"] = _config.ClientId;
            contentValues["client_secret"] = _config.ClientSecret;
            contentValues["username"] = _config.Username;
            contentValues["password"] = _config.Password;
            return contentValues;
        }

        internal SecureAuthAccessToken DeserializeResponseToAccessToken(string response)
        {
            var resultDict = DeserializeJson<Dictionary<string, object>>(response);

            var token = new SecureAuthAccessToken {
                AccessToken = (string)resultDict["access_token"],
                TokenType = (string)resultDict["token_type"],
                ExpiresIn = int.Parse((string)resultDict["expires_in"])
            };

            return token;
        }

        public virtual SecureAuthAccessToken GetAccessToken()
        {
            var contentValues = GetRequestContentForGetAccessToken();
            var content = new FormUrlEncodedContent(contentValues);

            // SecureAuth throws an exception if this isn't set. This may be able to go away when upgrading to .net 4.7
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // oidctoken.aspx should be able to be hard-coded because it should always be the same
            // regardless of which SecureAuth endpoint is being used.
            using (var result = _httpClient.PostAsync("oidctoken.aspx", content).Result)
            {
                var resultString = result.Content.ReadAsStringAsync()
                                         .Result;
                return DeserializeResponseToAccessToken(resultString);
            }
        }

        public void Dispose()
        {
            try
            {
                _httpClient?.Dispose();
            }
            finally
            {
                _httpClient = null;
            }
        }

        #endregion
    }
}
