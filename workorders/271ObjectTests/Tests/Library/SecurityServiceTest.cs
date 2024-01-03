using System.Linq;
using System.Web.Mvc;
using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using WorkOrders;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using ISecurityService = WorkOrders.Library.Permissions.ISecurityService;

namespace _271ObjectTests.Tests.Library
{
    /// <summary>
    /// Summary description for SecurityServiceTest.
    /// </summary>
    [TestClass]
    public class SecurityServiceTest : EventFiringTestClass
    {
        #region Private Members

        private IOperatingCenterRepository _operatingCenterRepository;
        private IEmployeeRepository _repository;
        
        private ISecurityService _target;
        private OperatingCenter[] data;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {

            base.EventFiringTestClassInitialize();
            _container = new Container();

            _mocks
                .DynamicMock(out _repository)
                .DynamicMock(out _operatingCenterRepository);

            _container.Inject(_repository);
            _container.Inject(_operatingCenterRepository);

            _target = SecurityService.Instance;

            var state = new State {
                Abbreviation = "NJ"
            };

            data = new [] {
                new OperatingCenter {
                    OpCntr = "NJ7",
                    OperatingCenterID = 10,
                    State = state
                },
                new OperatingCenter {
                    OpCntr = "NJ4",
                    OperatingCenterID = 14,
                    State = state
                },
                new OperatingCenter {
                    OpCntr = "NJ3",
                    OperatingCenterID = 13,
                    State = state
                }
            };
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            CleanOutInnerFields();

            base.EventFiringTestClassCleanup();
        }

        private void CleanOutInnerFields()
        {
            _target.SetHiddenFieldValueByName("_employeeRepository", null);
            _target.SetHiddenFieldValueByName("_operatingCenterRepository", null);
            _target.SetHiddenFieldValueByName("_employee", null);
            _target.SetHiddenFieldValueByName("_userHasAccess", null);
            _target.SetHiddenFieldValueByName("_adminOperatingCenters", null);
            _target.SetHiddenFieldValueByName("_all271OperatingCenters", null);
            _target.SetHiddenFieldValueByName("_isAdmin", null);
            _target.SetHiddenFieldValueByName("_defaultOperatingCenter", null);
            _target.SetHiddenFieldValueByName("_userOperatingCenters", null);
            //_target.SetHiddenFieldValueByName("_currentUser", null);
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestGetsDefaultOperatingCenterFromEmployee()
        {
            var operatingCenter = new OperatingCenter();
            var employee = new Employee {
                DefaultOperatingCenter = operatingCenter
            };
            
            var user = _mocks.DynamicMock<IUser>();

            _target.Init(user);
            _target.SetHiddenFieldValueByName("_employee", employee);
            _target.SetHiddenFieldValueByName("_isAdmin", false);
            _mocks.ReplayAll();
            
            Assert.AreSame(_target.DefaultOperatingCenter, operatingCenter);
        }

        [TestMethod]
        public void TestDefaultOperatingCenterIsInjectable()
        {
            var operatingCenter = new OperatingCenter();
            
            _target.SetHiddenFieldValueByName("_isAdmin", false);
            _target.SetHiddenFieldValueByName("_defaultOperatingCenter", operatingCenter);
            _mocks.ReplayAll();
            Assert.AreSame(_target.DefaultOperatingCenter, operatingCenter);
        }

        [TestMethod]
        public void TestDefaultOperatingCenterReturnsNullWhenIsAdminIsTrue()
        {
            var employee = new Employee();

            var user = _mocks.DynamicMock<IUser>();

            _target.Init(user);
            _target.SetHiddenFieldValueByName("_employee", employee);
            _target.SetHiddenFieldValueByName("_isAdmin", true);

            _mocks.ReplayAll();

            Assert.AreSame(_target.DefaultOperatingCenter, null);
        }

        [TestMethod]
        public void TestGetsEmployeeRepositoryFromIocContainer()
        {
            _container.Inject(_repository);

            _mocks.ReplayAll();

            // intended to be a protected property
            Assert.AreSame(_repository, _target.GetPropertyValueByName("EmployeeRepository"));
        }

        [TestMethod]
        public void TestGetsEmployeeFromEmployeeRepositoryUsingUserIdentityName()
        {
            var employee = new Employee();
            var user = _mocks.DynamicMock<IUser>();
            // Diamonds, dasies, snowflakes,
            var name = "That Guy";

            using (_mocks.Record())
            {
                SetupResult
                    .For(user.Name)
                    .Return(name);
                SetupResult
                    .For(_repository.GetEmployeeByUserName(name))
                    .Return(employee);
            }

            using (_mocks.Playback())
            {
                _target.Init(user);
                Assert.AreSame(employee, _target.Employee);
            }
        }

        [TestMethod]
        public void TestUserHasAccessReturnsTrueIfUserHas271AccessToAtLeastOneOperatingCenter()
        {
            _mocks.ReplayAll();

            _target.SetHiddenFieldValueByName("_adminOperatingCenters", new OperatingCenter[0]);
            _target.SetHiddenFieldValueByName("_userOperatingCenters", data);

            Assert.IsTrue(_target.UserHasAccess);
        }

        [TestMethod]
        public void TestUserHasAccessReturnsFalseIfUserDoesNotHave271AccessToAtLeastOneOperatingCenter()
        {
            _mocks.ReplayAll();

            _target.SetHiddenFieldValueByName("_adminOperatingCenters", new OperatingCenter[0]);
            _target.SetHiddenFieldValueByName("_userOperatingCenters", new OperatingCenter[0]);

            Assert.IsFalse(_target.UserHasAccess);
        }

        [TestMethod]
        public void TestAll271OperatingCentersPropertyReturnsMock()
        {
            var opCntrs = new OperatingCenter[0];
            var user = _mocks.DynamicMock<IUser>();

            using(_mocks.Record())
            {
                SetupResult.For(
                    _operatingCenterRepository.GetAll271OperatingCenters()).
                    Return(opCntrs);
            }
            using (_mocks.Playback())
            {
                _target.Init(user);
                Assert.AreSame(opCntrs,
                    _target.GetPropertyValueByName("All271OperatingCenters"));
            }
        }

        [TestMethod]
        public void TestAdminOperatingCentersReturnsCorrectMockedValue()
        {
            var opCntrs = new OperatingCenter[0];
            _mocks.ReplayAll();
            _target.SetHiddenFieldValueByName("_adminOperatingCenters", opCntrs);

            Assert.AreSame(opCntrs,_target.AdminOperatingCenters);
        }

        [TestMethod]
        public void TestAdminOperatingCentersIncludesCorrectAdminOperatingCentersForUser()
        {
            var user = _mocks.DynamicMock<IUser>();
            var permissions = _mocks.DynamicMock<IPermissionsObject>();
            _target.SetHiddenFieldValueByName("_all271OperatingCenters", data);

            using (_mocks.Record())
            {
                SetupResult.For(permissions.In("NJ7")).Return(true);
                SetupResult.For(permissions.In("NJ4")).Return(false);
                SetupResult.For(permissions.In("NJ3")).Return(false);

                SetupResult
                    .For(user.CanAdministrate(FieldServices.WorkManagement))
                    .Return(permissions);
            }
            using (_mocks.Playback())
            {
                _target.Init(user);
                var result = _target.AdminOperatingCenters.ToArray();
                Assert.IsTrue(result.Contains(data[0]));
                Assert.IsFalse(result.Contains(data[1]));
                Assert.IsFalse(result.Contains(data[2]));
            }
        }

        [TestMethod]
        public void TestIsAdminPropertyIsInjectable()
        {
            _mocks.ReplayAll();
            var bools = new[] { true, false };

            foreach (var bl in bools)
            {
                _target.SetHiddenFieldValueByName("_isAdmin", bl);
                Assert.AreEqual(bl, _target.IsAdmin);
            }
        }

        [TestMethod]
        public void TestIsAdminPropertyReturnsTrueWhenIsAdminOfAtLeastOneOperatingCenter()
        {
            // data has all the operating centers we need.
            var user = _mocks.DynamicMock<IUser>();
            var permissions = _mocks.DynamicMock<IPermissionsObject>();
            using (_mocks.Record())
            {
                SetupResult.For(permissions.In("NJ7")).Return(true);
                SetupResult.For(permissions.In("NJ4")).Return(false);
                SetupResult.For(permissions.In("NJ3")).Return(false);

                SetupResult
                    .For(user.CanAdministrate(FieldServices.WorkManagement))
                    .Return(permissions);
            }
            using (_mocks.Playback())
            {
                _target.Init(user);
                _target.SetHiddenFieldValueByName("_all271OperatingCenters",
                    data);
                Assert.IsTrue(_target.IsAdmin);
            }
        }
        
        [TestMethod]
        public void TestIsAdminPropertyReturnsFalseWhenIsNotAdminOfAtLeastOneOperatingCenter()
        {
            // data has all the operating centers we need.
            var user = _mocks.DynamicMock<IUser>();
            var permissions = _mocks.DynamicMock<IPermissionsObject>();
            using (_mocks.Record())
            {
                SetupResult.For(permissions.In("NJ7")).Return(false);
                SetupResult.For(permissions.In("NJ4")).Return(false);
                SetupResult.For(permissions.In("NJ3")).Return(false);

                SetupResult
                    .For(user.CanAdministrate(FieldServices.WorkManagement))
                    .Return(permissions);
            }
            using (_mocks.Playback())
            {
                _target.Init(user);
                _target.SetHiddenFieldValueByName("_all271OperatingCenters",
                    data);
                Assert.IsFalse(_target.IsAdmin);
            }
        }

        [TestMethod]
        public void TestUserOperatingCentersReturnsMockedValueIfProvided()
        {
            _mocks.ReplayAll();

            var arrData = data.ToArray();

            _target.SetHiddenFieldValueByName("_userOperatingCenters", arrData);

            Assert.AreSame(arrData, _target.UserOperatingCenters);
        }

        [TestMethod]
        public void TestUserOperatingCentersReturnsAdminOperatingCenterArrayIfIsAdmin()
        {
            _mocks.ReplayAll();

            var arrData = data.ToArray();

            _target.SetHiddenFieldValueByName("_adminOperatingCenters", arrData);
            _target.SetHiddenFieldValueByName("_isAdmin", true);

            for (var i = arrData.Length - 1; i >= 0; --i)
                Assert.AreSame(arrData[i], _target.UserOperatingCenters[i]);
        }

        [TestMethod]
        public void TestUserOperatingCentersCountReturnsUsersOperatingCentersCount()
        {
            var userOpCntrs = new[] { new OperatingCenter() };
            _target.SetHiddenFieldValueByName("_userOperatingCenters",
                userOpCntrs);
            Assert.AreEqual(userOpCntrs.Count(),
                _target.UserOperatingCentersCount);
            _mocks.ReplayAll();
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestInitSetsUserProperty()
        {
            var user = _mocks.DynamicMock<IUser>();

            _target.Init(user);

            Assert.AreSame(user, _target.CurrentUser);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestInitCallsResetPrivateMembersSetsPrivateMembersToNull()
        {
            var user = _mocks.DynamicMock<IUser>();
            _target.SetHiddenFieldValueByName("_isAdmin", true);
            _target.SetHiddenFieldValueByName("_userHasAccess", true);
            _target.SetHiddenFieldValueByName("_employee", new Employee());
            _target.SetHiddenFieldValueByName("_defaultOperatingCenter", new OperatingCenter());
            _target.SetHiddenFieldValueByName("_adminOperatingCenters", new OperatingCenter[] {});
            _target.SetHiddenFieldValueByName("_userOperatingCenters", new OperatingCenter[] { });
            _target.Init(user);
            Assert.IsNull(_target.GetHiddenFieldValueByName("_isAdmin"));
            Assert.IsNull(_target.GetHiddenFieldValueByName("_userHasAccess"));
            Assert.IsNull(_target.GetHiddenFieldValueByName("_employee"));
            Assert.IsNull(_target.GetHiddenFieldValueByName("_defaultOperatingCenter"));
            Assert.IsNull(_target.GetHiddenFieldValueByName("_adminOperatingCenters"));
            Assert.IsNull(_target.GetHiddenFieldValueByName("_userOperatingCenters"));
            _mocks.ReplayAll();
            
        }

        [TestMethod]
        public void TestGetEmployeeIDReturnsEmployeeEmployeeID()
        {
            var expected = 11111111;
            var employee = new Employee { EmployeeID = expected };
            _target.SetHiddenFieldValueByName("_employee", employee);
            Assert.AreEqual(expected, _target.GetEmployeeID());
            _mocks.ReplayAll();
        }

        #endregion

        #region Static Properties

        [TestMethod]
        public void TestInstancePropertyUsersMockedValueIfPresent()
        {
            var expected = _mocks.DynamicMock<ISecurityService>();
            typeof(SecurityService).SetHiddenStaticFieldValueByName(
                "_instance", expected);
            Assert.AreSame(expected, SecurityService.Instance);
            _mocks.ReplayAll();
            typeof(SecurityService).SetHiddenStaticFieldValueByName(
                "_instance", null);
        }

        #endregion
        
        #region Static Methods
        [TestMethod]
        public void TestSelectUserOperatingCentersReturnsInstanceUserOperatingCenters()
        {
            var userOpCntrs = new[] { new OperatingCenter() };
            var instance = _mocks.DynamicMock<ISecurityService>();

            typeof(SecurityService).SetHiddenStaticFieldValueByName(
                "_instance", instance);

            using (_mocks.Record())
            {
                SetupResult.For(instance.UserOperatingCenters).Return(userOpCntrs);
            }
            using (_mocks.Playback())
            {
                var results = SecurityService.SelectUserOperatingCenters();
                Assert.AreSame(userOpCntrs, results);
            }

            typeof(SecurityService).SetHiddenStaticFieldValueByName(
                "_instance", null);
        }
        
        #endregion
    }
}
