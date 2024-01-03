using System.Security.Principal;
using FluentNHibernate.Utils;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhino.Mocks;
using StructureMap;

namespace MMSINC.CoreTest.Utilities.Permissions
{
    /// <summary>
    /// Summary description for SiteUserWrapperTest.
    /// </summary>
    [TestClass]
    public class SiteUserWrapperTest
    {
        #region Private Members

        private Mock<IPrincipal> _wrappedUser;
        private SiteUserWrapper _target;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void EventFiringTestClassInitialize()
        {
            _container = new Container();
            _wrappedUser = new Mock<IPrincipal>();

            _target = new TestSiteUserWrapper(_wrappedUser.Object, _container.GetInstance<PermissionsObjectFactory>());
        }

        [TestCleanup]
        public void EventFiringTestClassCleanup()
        {
            _wrappedUser.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsWrappedUser()
        {
            Assert.AreSame(_wrappedUser.Object,
                _target.GetHiddenFieldValueByName("_innerUser"));
        }

        [TestMethod]
        public void TestIsInRoleIsCalledOnInnerUserWhenCallingIUserIsInRole()
        {
            var expected = "whatever";

            _wrappedUser.Setup(x => x.IsInRole(expected)).Returns(true);

            Assert.IsTrue(_target.IsInRole(expected));
        }

        [TestMethod]
        public void TestPermissionsMethodsReturnPermissionsObjectsWithCorrectValues()
        {
            _container.Inject(new Mock<IRoleManager>().Object);
            var modulePermissions = new ModulePermissions("foo", "bar");

            var permissionsObject = _target.CanAdd(modulePermissions);

            Assert.AreSame(_target, permissionsObject.User);
            Assert.AreSame(modulePermissions, permissionsObject.SpecificPermissions);
            Assert.AreEqual(ModuleAction.Add, permissionsObject.Action);

            permissionsObject = _target.CanRead(modulePermissions);

            Assert.AreSame(_target, permissionsObject.User);
            Assert.AreSame(modulePermissions, permissionsObject.SpecificPermissions);
            Assert.AreEqual(ModuleAction.Read, permissionsObject.Action);

            permissionsObject = _target.CanEdit(modulePermissions);

            Assert.AreSame(_target, permissionsObject.User);
            Assert.AreSame(modulePermissions, permissionsObject.SpecificPermissions);
            Assert.AreEqual(ModuleAction.Edit, permissionsObject.Action);

            permissionsObject = _target.CanDelete(modulePermissions);

            Assert.AreSame(_target, permissionsObject.User);
            Assert.AreSame(modulePermissions, permissionsObject.SpecificPermissions);
            Assert.AreEqual(ModuleAction.Delete, permissionsObject.Action);

            permissionsObject = _target.CanAdministrate(modulePermissions);

            Assert.AreSame(_target, permissionsObject.User);
            Assert.AreSame(modulePermissions, permissionsObject.SpecificPermissions);
            Assert.AreEqual(ModuleAction.Administrate, permissionsObject.Action);
        }

        [TestMethod]
        public void TestIdentityPropertyReturnsMockedIdentityValue()
        {
            var expected = new Mock<IIdentity>();
            _target = new TestSiteUserWrapperBuilder(_wrappedUser.Object,
                _container.GetInstance<PermissionsObjectFactory>()).WithIdentity(expected.Object);

            Assert.AreSame(expected.Object, _target.Identity);
        }

        [TestMethod]
        public void TestNameReturnsMockedNameValue()
        {
            var expected = "mcUser";
            _target = new TestSiteUserWrapperBuilder(_wrappedUser.Object,
                _container.GetInstance<PermissionsObjectFactory>()).WithName(expected);

            Assert.AreSame(expected, _target.Name);
        }

        [TestMethod]
        public void TestNamePropertyReturnsNameValueFromIdentityPropertyIfNotNull()
        {
            var expected = "testuser";
            var identity = new Mock<IIdentity>();
            _wrappedUser.Setup(x => x.Identity).Returns(identity.Object);
            identity.Setup(x => x.Name).Returns(expected);

            Assert.AreSame(expected, _target.Name);
        }
    }

    internal class TestSiteUserWrapperBuilder : TestDataBuilder<TestSiteUserWrapper>
    {
        #region Private Members

        public readonly IPrincipal _innerUser;
        private readonly IPermissionsObjectFactory _permissionsObjectFactory;
        private IIdentity _identity;
        private string _name;

        #endregion

        #region Constructors

        internal TestSiteUserWrapperBuilder(IPrincipal wrappedUser, IPermissionsObjectFactory permissionsObjectFactory)
        {
            _innerUser = wrappedUser;
            _permissionsObjectFactory = permissionsObjectFactory;
        }

        #endregion

        #region Exposed Methods

        public override TestSiteUserWrapper Build()
        {
            var obj = new TestSiteUserWrapper(_innerUser, _permissionsObjectFactory);
            if (!string.IsNullOrEmpty(_name))
                obj.SetName(_name);
            if (_identity != null)
                obj.SetIdentity(_identity);
            return obj;
        }

        public TestSiteUserWrapperBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public TestSiteUserWrapperBuilder WithIdentity(IIdentity identity)
        {
            _identity = identity;
            return this;
        }

        #endregion
    }

    internal class TestSiteUserWrapper : SiteUserWrapper
    {
        #region Constructors

        public TestSiteUserWrapper(IPrincipal innerUser, IPermissionsObjectFactory permissionsObjectFactory) : base(
            innerUser, permissionsObjectFactory) { }

        #endregion

        #region ExposedMethods

        public void SetName(string name)
        {
            _name = name;
        }

        public void SetIdentity(IIdentity identity)
        {
            _identity = identity;
        }

        #endregion
    }
}
