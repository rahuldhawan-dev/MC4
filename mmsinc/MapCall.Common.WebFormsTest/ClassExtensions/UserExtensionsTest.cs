using System.Security.Principal;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCall.Common.WebFormsTest.ClassExtensions
{
    /// <summary>
    /// Summary description for UserExtensionsTest
    /// </summary>
    [TestClass]
    public class UserExtensionsTest
    {
        #region Private Members

        private Mock<IPrincipal> _iPrincipalMock;
        private Mock<IIdentity> _iIdentityMock;
        private IContainer _container;

        #endregion

        #region Tests

        [TestInitialize]
        public void UserExtensionsTestInitialize()
        {
            _iPrincipalMock = new Mock<IPrincipal>();
            _iIdentityMock = new Mock<IIdentity>();

            _iPrincipalMock.SetupGet(u => u.Identity)
                           .Returns(_iIdentityMock.Object);

            _container = new Container();
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        [TestMethod]
        public void TestIsAdminReturnsAuthenticationServiceCurrentUserIsAdmin()
        {
            var authServ = new Mock<IAuthenticationService>();
            _container.Inject(authServ.Object);

            authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            Assert.IsTrue(_iPrincipalMock.Object.IsAdmin());
            authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            Assert.IsFalse(_iPrincipalMock.Object.IsAdmin());
        }

        #endregion
    }
}
