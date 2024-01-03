using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class SystemDeliveryEntryHelperTest
    {
        #region Private Members

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private IContainer _container;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _today;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(e => { e.For<IUserRepository>().Mock(); });
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _today = _dateTimeProvider.Object.GetCurrentDate();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authServ.Object);
            _user = new User {
                Roles = new List<Role>()
            };
        }

        #endregion

        #region Private Methods

        private Role CreateRole(RoleModules module, RoleActions action = RoleActions.Read,
            OperatingCenter opCenter = null)
        {
            var role = new Role {
                User = _user,
                Module = new Module(),
                Action = new RoleAction(),
                OperatingCenter = opCenter
            };

            role.Module.SetPropertyValueByName("Id", (int)module);
            role.Action.SetPropertyValueByName("Id", (int)action);

            return role;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void SystemDeliveryEntryIsNotReversable()
        {
            var thirdDayOfNextMonth = new DateTime(_today.Year, _today.Month, 3).AddMonths(1);
            var fourthDayOfNextMonth = new DateTime(_today.Year, _today.Month, 4).AddMonths(1);
            _user.Roles.Clear();

            var result =
                SystemDeliveryEntryHelpers.IsSystemDeliveryEntryReversable(fourthDayOfNextMonth, thirdDayOfNextMonth,
                    _user);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SystemDeliveryEntryIsReversableWhenCurrentDateIsBeforeTheThirdDayOfTheFollowingMonth()
        {
            var thirdDayOfNextMonth = new DateTime(_today.Year, _today.Month, 3).AddMonths(1);
            var role = CreateRole(RoleModules.ProductionSystemDeliveryEntry, RoleActions.Add);
            _user.Roles.Add(role);

            var result = SystemDeliveryEntryHelpers.IsSystemDeliveryEntryReversable(_today, thirdDayOfNextMonth, _user);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SystemDeliveryEntryIsReversableWhenUserIsAdmin()
        {
            _user.IsAdmin = true;
            _user.Roles.Clear();

            var result = SystemDeliveryEntryHelpers.IsSystemDeliveryEntryReversable(
                _dateTimeProvider.Object.GetCurrentDate(), DateTime.MinValue, _user);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SystemDeliveryEntryIsReversableWhenUserIsApprover()
        {
            var role = CreateRole(RoleModules.ProductionSystemDeliveryApprover, RoleActions.Add);
            _user.Roles.Add(role);

            var result = SystemDeliveryEntryHelpers.IsSystemDeliveryEntryReversable(
                _dateTimeProvider.Object.GetCurrentDate(), DateTime.MinValue, _user);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SystemDeliveryEntryIsReversableWhenUserIsUserAdmin()
        {
            var role = CreateRole(RoleModules.ProductionSystemDeliveryApprover, RoleActions.UserAdministrator);
            _user.Roles.Add(role);

            var result = SystemDeliveryEntryHelpers.IsSystemDeliveryEntryReversable(
                _dateTimeProvider.Object.GetCurrentDate(), DateTime.MinValue, _user);

            Assert.IsTrue(result);
        }

        #endregion
    }
}
