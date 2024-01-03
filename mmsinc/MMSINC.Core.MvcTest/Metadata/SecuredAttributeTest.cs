using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using Moq;
using MMSINC.Metadata;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class SecuredAttributeTest 
    {
        #region Fields

        private Mock<IAuthenticationService<TestUser>> _authServ;
        private SecuredAttribute _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _authServ = new Mock<IAuthenticationService<TestUser>>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestUserCanEditReturnsTrueIfAppliesToAdminsIsFalseAndUserIsSiteAdmin()
        {
            _target = new SecuredAttribute { AppliesToAdmins = false };
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            Assert.IsTrue(_target.UserCanEdit(_authServ.Object));

            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            Assert.IsFalse(_target.UserCanEdit(_authServ.Object));
        }

        [TestMethod]
        public void TestUserCanEditReturnsFalseIfAppliesToAdminIsTrueRegardlessOfTheUserBeingSiteAdmin()
        {
            _target = new SecuredAttribute { AppliesToAdmins = true };
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            Assert.IsFalse(_target.UserCanEdit(_authServ.Object));
        }

        #endregion
    }
}
