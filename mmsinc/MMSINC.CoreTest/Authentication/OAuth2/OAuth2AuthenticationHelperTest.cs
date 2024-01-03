using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication.OAuth2;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using Newtonsoft.Json;

namespace MMSINC.CoreTest.Authentication.OAuth2
{
    [TestClass]
    public class OAuth2AuthenticationHelperTest
    {
        #region Private Members

        private IOAuth2AuthenticationHelper _target;
        private OAuth2Config _config;
        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<IHttpClient> _httpClient;
        private Mock<IOAuth2TokenValidator> _validator;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _config = new OAuth2Config(
                "clientId",
                "clientSecret",
                "oktaDomain",
                "redirectUri",
                "postLogoutRedirectUri");
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClient = _httpClientFactory.SetupMock(
                x => x.Build(It.Is<HttpClientSettings>(
                    s => s.Url == _config.OktaDomain)));
            _validator = new Mock<IOAuth2TokenValidator>();

            _target = new OAuth2AuthenticationHelper(_config, _httpClientFactory.Object, _validator.Object);
        }

        #endregion

        #region Nested Type: AuthorizeUrl

        private class AuthorizeUrl
        {
            #region Properties

            public string Url { get; }
            public string ClientId { get; }
            public string ResponseType { get; }
            public string Scope { get; }
            public string RedirectUri { get; }
            public string State { get; }

            #endregion

            #region Constructors

            public AuthorizeUrl(
                string url,
                string clientId,
                string responseType,
                string scope,
                string redirectUri,
                string state)
            {
                Url = url;
                ClientId = clientId;
                ResponseType = responseType;
                Scope = scope;
                RedirectUri = redirectUri;
                State = state;
            }

            #endregion

            #region Exposed Methods

            public static AuthorizeUrl FromString(string url)
            {
                var split = url.Split('?');

                var query = HttpUtility.ParseQueryString(split[1]);
                return new AuthorizeUrl(
                    split[0],
                    query["client_id"],
                    query["response_type"],
                    query["scope"],
                    query["redirect_uri"],
                    query["state"]);
            }

            #endregion
        }

        #endregion
        
        #region DoAuthRedirect

        [TestMethod]
        public void TestDoAuthRedirectCallsRedirectFnWithGeneratedOAuthUrl()
        {
            var expectedUrl = new AuthorizeUrl($"{_config.OktaDomain}/oauth2/default/v1/authorize",
                _config.ClientId,
                "code",
                "openid profile",
                _config.RedirectUri,
                null);

            var fnCalled = false;
            void RedirectFn(string url)
            {
                var urlObj = AuthorizeUrl.FromString(url);

                Assert.AreEqual(expectedUrl.Url, urlObj.Url);
                Assert.AreEqual(expectedUrl.ClientId, urlObj.ClientId);
                Assert.AreEqual(expectedUrl.ResponseType, urlObj.ResponseType);
                Assert.AreEqual(expectedUrl.Scope, urlObj.Scope);
                Assert.AreEqual(expectedUrl.RedirectUri, urlObj.RedirectUri);

                fnCalled = true;
            }
            
            _target.DoAuthRedirect(RedirectFn, null);
            
            Assert.IsTrue(fnCalled);
        }

        [TestMethod]
        public void TestDoAuthRedirectUsesReturnUrlAsStateParam()
        {
            var returnUrl = "/some/return/url.aspx";
            var expectedUrl = new AuthorizeUrl($"{_config.OktaDomain}/oauth2/default/v1/authorize",
                _config.ClientId,
                "code",
                "openid profile",
                _config.RedirectUri,
                returnUrl);

            var fnCalled = false;

            void RedirectFn(string url)
            {
                var urlObj = AuthorizeUrl.FromString(url);
                Assert.AreEqual(expectedUrl.State, urlObj.State);

                fnCalled = true;
            }
                    
            _target.DoAuthRedirect(RedirectFn, returnUrl);
            
            Assert.IsTrue(fnCalled);
        }

        [TestMethod]
        public void TestDoAuthRedirectDoesNotIncludeNullStateParameter()
        {
            var fnCalled = false;

            void RedirectFn(string url)
            {
                var urlObj = AuthorizeUrl.FromString(url);
                Assert.AreEqual("/", urlObj.State);

                fnCalled = true;
            }
                    
            _target.DoAuthRedirect(RedirectFn, null);
            
            Assert.IsTrue(fnCalled);
        }
        
        #endregion
        
        #region HandleAuthenticationResult

        [TestMethod]
        public void TestHandleAuthenticationResultThrowsExceptionWhenQueryStringHasNoCodeValue()
        {
            Assert.ThrowsException<AuthenticationException>(
                () => _target.HandleAuthenticationResult(new NameValueCollection(0)));
        }

        [TestMethod]
        public void TestHandleAuthenticationResultRetrievesUsernameFromSuccessfulTokenResponse()
        {
            const string code = nameof(code);
            const string name = nameof(name);
            var idToken = new OAuth2IdToken {
                Name = name
            };
            var accessToken = new OAuth2AccessTokenResponse {
                IdToken = "." + JsonConvert.SerializeObject(idToken).ToBase64String() + "."
            };

            _httpClient.Setup(x => x.PostAsync(
                            $"{_config.OktaDomain}/oauth2/default/v1/token",
                            It.IsAny<FormUrlEncodedContent>()))
                       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) {
                            Content = new StringContent(JsonConvert.SerializeObject(accessToken))
                        });

            var result = _target.HandleAuthenticationResult(new NameValueCollection {
                { nameof(code), code }
            });
            
            Assert.AreEqual(name, result.Username);
        }

        [TestMethod]
        public void TestHandleAuthenticationResultThrowsExceptionWhenTokenRequestUnsuccessful()
        {
            const string code = nameof(code);

            _httpClient.Setup(x => x.PostAsync(
                            $"{_config.OktaDomain}/oauth2/default/v1/token",
                            It.IsAny<FormUrlEncodedContent>()))
                       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest) {
                            Content = new StringContent(JsonConvert.SerializeObject(new {foo = "bar"}))
                        });

            Assert.ThrowsException<AuthenticationException>(
                () => _target.HandleAuthenticationResult(new NameValueCollection {
                    { nameof(code), code }
                }));
        }

        [TestMethod]
        public void TestHandleAuthenticationResultIncludesReturnUrlFromStateParameterIfAvailable()
        {
            const string code = nameof(code);
            const string name = nameof(name);
            var returnUrl = "/some/path/to/a/thing.aspx";
            var idToken = new OAuth2IdToken {
                Name = name
            };
            var accessToken = new OAuth2AccessTokenResponse {
                IdToken = "." + JsonConvert.SerializeObject(idToken).ToBase64String() + "."
            };

            _httpClient.Setup(x => x.PostAsync(
                            $"{_config.OktaDomain}/oauth2/default/v1/token",
                            It.IsAny<FormUrlEncodedContent>()))
                       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) {
                            Content = new StringContent(JsonConvert.SerializeObject(accessToken))
                        });

            var result = _target.HandleAuthenticationResult(new NameValueCollection {
                { nameof(code), code },
                { "state", HttpUtility.UrlEncode(returnUrl) }
            });

            Assert.AreEqual(returnUrl, result.ReturnUrl);
        }

        [TestMethod]
        public void TestHandleAuthenticationResultPropagatesAnyExceptionThrownByTheValidator()
        {
            const string code = nameof(code);
            var accessToken = new OAuth2AccessTokenResponse {
                IdToken = "..signature"
            };

            _httpClient.Setup(x => x.PostAsync(
                            $"{_config.OktaDomain}/oauth2/default/v1/token",
                            It.IsAny<FormUrlEncodedContent>()))
                       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) {
                            Content = new StringContent(JsonConvert.SerializeObject(accessToken))
                        });
            _validator.Setup(x => x.Validate(It.IsAny<OAuth2IdToken>()))
                      .Throws(new AuthenticationException());

            Assert.ThrowsException<AuthenticationException>(
                () => _target.HandleAuthenticationResult(new NameValueCollection {
                    { nameof(code), code }
                }));
        }
        
        #endregion
    }
}
