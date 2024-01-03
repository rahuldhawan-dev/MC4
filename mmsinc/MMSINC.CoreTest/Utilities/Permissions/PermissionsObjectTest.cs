using MMSINC.ClassExtensions;
using MMSINC.Utilities.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MMSINC.CoreTest.Utilities.Permissions
{
    [TestClass]
    public class PermissionsObjectTest
    {
        private IRoleManager _originalRoleManager;
        private Mock<IRoleManager> _roleManagerMock;
        private PermissionsObject _target;
        private IContainer _container;

        [TestInitialize]
        public void PermissionsObjectTestInitialize()
        {
            _roleManagerMock = new Mock<IRoleManager>();
            _target = new PermissionsObject(_roleManagerMock.Object, null, null, ModuleAction.Add);
            _container = new Container();
            _container.Inject(_roleManagerMock.Object);
        }

        [TestMethod]
        public void TestInAnyReturnsValueFromCallToUserIsInRole()
        {
            _roleManagerMock.Setup(x => x.UserIsInRole(_target)).Returns(false);
            Assert.IsFalse(_target);

            _roleManagerMock.VerifyAll();

            _roleManagerMock.Setup(x => x.UserIsInRole(_target)).Returns(true);
            Assert.IsTrue(_target);

            _roleManagerMock.VerifyAll();
        }

        [TestMethod]
        public void TestInReturnsValueFromCallToUserIsInRoleWithOperatingCenter()
        {
            var opCenter = "foo";

            _roleManagerMock.Setup(
                x => x.UserIsInRoleWithOperatingCenter(_target, opCenter)).Returns(false);
            Assert.IsFalse(_target.In(opCenter));

            _roleManagerMock.VerifyAll();

            _roleManagerMock.Setup(
                x => x.UserIsInRoleWithOperatingCenter(_target, opCenter)).Returns(true);
            Assert.IsTrue(_target.In(opCenter));

            _roleManagerMock.VerifyAll();
        }
    }
}
