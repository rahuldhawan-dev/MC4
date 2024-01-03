using System;
using System.Security.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication.OAuth2;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;

namespace MMSINC.CoreTest.Authentication.OAuth2
{
    [TestClass]
    public class OAuth2TokenValidatorTest
    {
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;
        private OAuth2Config _config;
        private IOAuth2TokenValidator _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now);
            _config = new OAuth2Config(
                "clientId",
                "clientSecret",
                "oktaDomain",
                "redirectUri",
                "postLogoutRedirectUri");
            
            _target = new OAuth2TokenValidator(_config, _dateTimeProvider);
        }

        private OAuth2IdToken GetValidToken()
        {
            return new OAuth2IdToken {
                Issuer = _config.OktaDomain,
                IssuedAtTime = (int)(_now.SecondsSinceEpoch() - 1),
                ExpiryTime = (int)(_now.SecondsSinceEpoch() + 1)
            };
        }

        [TestMethod]
        public void TestValidateDoesNotThrowWhenAllItemsAreOk()
        {
            MyAssert.DoesNotThrow(() => _target.Validate(GetValidToken()));
        }

        [TestMethod]
        public void TestValidateDoesNotThrowWhenIssuerStartsWithConfiguredOktaDomain()
        {
            var token = GetValidToken();
            token.Issuer += "/some/other/junk";
            
            MyAssert.DoesNotThrow(() => _target.Validate(token));
        }

        [TestMethod]
        public void TestValidateThrowsWhenIssuerDoesNotMatchConfiguredOktaDomain()
        {
            var token = GetValidToken();
            token.Issuer = "not the okta domain";
            
            Assert.ThrowsException<AuthenticationException>(
                () => _target.Validate(token));
        }

        [TestMethod]
        public void TestValidateThrowsWhenExpiryTimeIsInPast()
        {
            var token = GetValidToken();
            token.ExpiryTime = (int)(_now.SecondsSinceEpoch() - 1);
            
            Assert.ThrowsException<AuthenticationException>(
                () => _target.Validate(token));
        }
    }
}
