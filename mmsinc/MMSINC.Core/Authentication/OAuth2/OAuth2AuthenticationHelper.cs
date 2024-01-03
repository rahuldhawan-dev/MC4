using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Web;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Utilities;
using Newtonsoft.Json;

namespace MMSINC.Authentication.OAuth2
{
    /// <inheritdoc />
    /// <remarks>
    /// Disposing of this implementation has potential consequences because of dependencies it has also being disposed.
    /// When used in a long-running application this should be registered with IoC as a singleton.  See
    /// <see cref="Dispose"/> for more information.
    /// </remarks>
    public class OAuth2AuthenticationHelper : IOAuth2AuthenticationHelper
    {
        #region Private Members
        
        private readonly OAuth2Config _config;
        private readonly IHttpClient _httpClient;
        private readonly IOAuth2TokenValidator _validator;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Constructor for the <see cref="OAuth2AuthenticationHelper"/> class.
        /// </summary>
        public OAuth2AuthenticationHelper(
            OAuth2Config config,
            IHttpClientFactory httpClient,
            IOAuth2TokenValidator validator)
        {
            _config = config;
            _httpClient = httpClient.Build(new HttpClientSettings {
                Url = _config.OktaDomain
            });
            _validator = validator;
        }
        
        #endregion
        
        #region Private Methods

        private string GetApiUrl(string endpoint)
        {
            return $"{_config.OktaDomain}/oauth2/default/v1/{endpoint}";
        }

        private bool TryGetAccessToken(
            NameValueCollection queryString,
            out OAuth2AccessTokenResponse token,
            out object error)
        {
            if (!queryString.ContainsKey("code"))
            {
                throw new AuthenticationException("No 'code' value provided from auth redirect");
            }

            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "grant_type", "authorization_code" },
                { "redirect_uri", _config.RedirectUri },
                { "code", queryString["code"] },
                { "client_id", _config.ClientId },
                { "client_secret", _config.ClientSecret },
            });
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var response = _httpClient.PostAsync(GetApiUrl("token"), content).Result)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    token = JsonConvert.DeserializeObject<OAuth2AccessTokenResponse>(responseBody);
                    error = null;
                    return true;
                }

                error = JsonConvert.DeserializeObject(responseBody);
                token = null;
                return false;
            }
        }

        private OAuth2IdToken ParseTokenPayload(string tokenPayload)
        {
            var payloadBytes = Convert.FromBase64String(
                tokenPayload.PadRight(
                    tokenPayload.Length + (4 - tokenPayload.Length % 4) % 4, '='));
            var tokenJson = Encoding.UTF8.GetString(payloadBytes);
            return JsonConvert.DeserializeObject<OAuth2IdToken>(tokenJson);
        }

        private string GetRedirectUrl(NameValueCollection queryString)
        {
            string returnUrl;
            if (!queryString.ContainsKey("state") ||
                string.IsNullOrWhiteSpace(returnUrl = queryString["state"]) ||
                (returnUrl = HttpUtility.UrlDecode(returnUrl)) == "/")
            {
                return null;
            }

            return returnUrl;
        }

        private (string Header, string Payload, string Signature) SplitToken(string token)
        {
            var split = token.Split('.');

            return (split[0], split[1], split[2]);
        }

        private void ValidateIdToken(OAuth2IdToken token)
        {
            _validator.Validate(token);
        }

        #endregion

        #region Exposed Methods

        /// <inheritdoc />
        public void DoAuthRedirect(Action<string> redirectFn, string returnUrl)
        {
            var queryParts = new Dictionary<string, string> {
                { "client_id", _config.ClientId },
                { "response_type", "code" },
                { "scope", HttpUtility.UrlEncode("openid profile") },
                { "redirect_uri", HttpUtility.UrlEncode(_config.RedirectUri) },
                { "state", HttpUtility.UrlEncode(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl) },
            }.Select(pair => $"{pair.Key}={pair.Value}").ToArray();
            var queryString = "?" + string.Join("&", queryParts);

            redirectFn(GetApiUrl($"authorize{queryString}"));
        }

        /// <inheritdoc />
        public OAuth2AuthenticationResult HandleAuthenticationResult(NameValueCollection queryString)
        {
            if (queryString.ContainsKey("error"))
            {
                return new OAuth2AuthenticationResult(HttpUtility.UrlDecode(queryString["error_description"]));
            }
            
            if (TryGetAccessToken(queryString, out var tokenResponse, out var error))
            {
                var (header, payload, signature) = SplitToken(tokenResponse.IdToken);
                var idToken = ParseTokenPayload(payload);

                ValidateIdToken(idToken);
                
                return new OAuth2AuthenticationResult(idToken.Name, GetRedirectUrl(queryString));
            }

            throw new AuthenticationException();
        }

        /// <inheritdoc />
        /// <remarks>
        /// This will dispose of the <see cref="IHttpClient"/> instance, which should only need to be done at
        /// application exit.  Thus, <see cref="OAuth2AuthenticationHelper"/> should be registered with IoC as a
        /// singleton.
        /// </remarks>
        public void Dispose()
        {
            _httpClient.Dispose();
        }
        
        #endregion
    }
}
