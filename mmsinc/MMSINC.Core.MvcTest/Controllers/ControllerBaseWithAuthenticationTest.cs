using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Controllers
{
    [TestClass]
    public class ControllerBaseWithAuthenticationTest
    {
        #region Private Members

        private Mock<IRepository<TestUser>> _userRepository;
        private Mock<IAuthenticationService<TestUser>> _authenticationSerivce;
        private TestAuthenticatedController _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(e => e.For<IViewModelFactory>().Use<ViewModelFactory>());
            _userRepository = new Mock<IRepository<TestUser>>();
            _authenticationSerivce = new Mock<IAuthenticationService<TestUser>>();
            _container.Inject(_userRepository.Object);
            _container.Inject(_authenticationSerivce.Object);
            _target = _container.GetInstance<TestAuthenticatedController>();
        }

        #endregion

        #region AuthenticationService

        [TestMethod]
        public void TestRetrievesAuthenticationServiceFromObjectFactory()
        {
            Assert.AreSame(_authenticationSerivce.Object, _target.AuthenticationService);
        }

        #endregion
    }

    public class TestAuthenticatedController : ControllerBaseWithAuthentication<TestUser, TestUser>
    {
        public TestAuthenticatedController(
            ControllerBaseWithAuthenticationArguments<IRepository<TestUser>, TestUser, TestUser> args) : base(args) { }
    }
}
