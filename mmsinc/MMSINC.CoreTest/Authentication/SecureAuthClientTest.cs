using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;

namespace MMSINC.CoreTest.Authentication
{
    [TestClass]
    public class SecureAuthClientTest
    {
        #region Fields

        private SecureAuthClient _target;
        private SecureAuthClientConfiguration _config;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _config = new SecureAuthClientConfiguration();
            _config.BaseEndpointUrl = "http://www.some.url/BaseUrl/";
            _target = new SecureAuthClient(_config);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetAccessTokenSetsupExpectedRequestHeaders()
        {
            // NOTE: This test can't directly call GetAccessToken due to the use
            // of the unmockable HttpClient.

            _config.ClientId = "some client id";
            _config.ClientSecret = "some client secret";
            _config.Password = "some password";
            _config.Username = "some username";

            var result = _target.GetRequestContentForGetAccessToken();
            Assert.AreEqual("some client id", result["client_id"]);
            Assert.AreEqual("some client secret", result["client_secret"]);
            Assert.AreEqual("some password", result["password"]);
            Assert.AreEqual("some username", result["username"]);
            Assert.AreEqual("password", result["grant_type"]);
            Assert.AreEqual("openid", result["scope"]);
        }

        [TestMethod]
        public void TestGetAccessTokenDeserializesResponseCorrectly()
        {
            // NOTE: This test can't directly call GetAccessToken due to the use
            // of the unmockable HttpClient.

            var expectedResponse =
                @"{ access_token: ""some access token"", token_type: ""some token type"", expires_in: ""3600"" }";
            var result = _target.DeserializeResponseToAccessToken(expectedResponse);

            Assert.AreEqual("some access token", result.AccessToken);
            Assert.AreEqual("some token type", result.TokenType);
            Assert.AreEqual(3600, result.ExpiresIn);
        }

        #endregion
    }
}
