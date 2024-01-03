using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class SecureFormTokenServiceTest
    {
        private IContainer _container;
        private SecureFormTokenService<SecureFormToken, SecureFormDynamicValue> _target;
        private Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>> _tokenRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _target = _container.GetInstance<SecureFormTokenService<SecureFormToken, SecureFormDynamicValue>>();
        }

        private void InitializeContainer(ConfigurationExpression e)
        {
            _tokenRepository = e.For<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>().Mock();
        }

        [TestMethod]
        public void TestCreateTokenWithRouteValuesCreatesTokenWithRouteValues()
        {
            _tokenRepository.Setup(x => x.Save(It.IsAny<SecureFormToken>())).Returns(new SecureFormToken());

            var result = _target.CreateTokenWithRouteValues("bar", "foo", 123, new Dictionary<string, object>());

            _tokenRepository.Verify(x => x.Save(It.Is<SecureFormToken>(t =>
                t.Action == "bar" && t.Controller == "foo" && t.Area == null && t.UserId == 123)));
        }

        [TestMethod]
        public void TestCreateTokenWithRouteValuesIncludesAreaIfProvided()
        {
            _tokenRepository.Setup(x => x.Save(It.IsAny<SecureFormToken>())).Returns(new SecureFormToken());

            var result =
                _target.CreateTokenWithRouteValues("baz", "bar", 321, new Dictionary<string, object> {{"area", "foo"}});

            _tokenRepository.Verify(x => x.Save(It.Is<SecureFormToken>(t =>
                t.Action == "baz" && t.Controller == "bar" && t.Area == "foo" && t.UserId == 321)));
        }
    }
}
