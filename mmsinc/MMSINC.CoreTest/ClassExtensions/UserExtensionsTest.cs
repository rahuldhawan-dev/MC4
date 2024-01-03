using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using Moq;

namespace MMSINC.CoreTest.ClassExtensions
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
        private static readonly IEnumerable<string> NON_ADMIN_USERS =
            new HashSet<string> {
                "Joey", "Johnny", "DeeDee", "Marky"
            }; 

        #endregion

        [TestInitialize]
        public void UserExtensionsTestInitialize()
        {
            _iPrincipalMock = new Mock<IPrincipal>();
            _iIdentityMock = new Mock<IIdentity>();

            _iPrincipalMock.SetupGet(u => u.Identity)
                .Returns(_iIdentityMock.Object);
        }

        [TestMethod]
        public void TestIsAdminReturnsTrueIfUsersNameIsInListOfAdminUserNames()
        {
            foreach (var userName in User.ADMIN_USER_NAMES)
            {
                _iIdentityMock
                    .SetupGet(i => i.Name)
                    .Returns(userName);

                Assert.IsTrue(_iPrincipalMock.Object.IsAdmin());

                _iPrincipalMock.VerifyAll();
            }

            foreach (var userName in NON_ADMIN_USERS)
            {
                _iIdentityMock
                    .SetupGet(i => i.Name)
                    .Returns(userName);

                Assert.IsFalse(_iPrincipalMock.Object.IsAdmin());

                _iPrincipalMock.VerifyAll();
            }
        }

        [TestMethod]
        public void TestIsAKevinReturnsTrueIfUsersNameIsInListOfAdminUserNames()
        {
            foreach (var userName in User.KEVINS)
            {
                _iIdentityMock
                    .SetupGet(i => i.Name)
                    .Returns(userName);

                Assert.IsTrue(_iPrincipalMock.Object.IsAKevin());

                _iPrincipalMock.VerifyAll();
            }

            foreach (var userName in NON_ADMIN_USERS)
            {
                _iIdentityMock
                    .SetupGet(i => i.Name)
                    .Returns(userName);

                Assert.IsFalse(_iPrincipalMock.Object.IsAKevin());

                _iPrincipalMock.VerifyAll();
            }
        }
    }
}